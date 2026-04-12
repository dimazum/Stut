import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { BehaviorSubject, Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { NotificationService } from './notification.service';
import { getErrorMessage } from './utils/utils';

export interface ChatMessage {
  id: number;
  senderId: string;
  receiverId: string;
  message: string;
  sentAt: string;
  readAt? :string;
}

interface ChatUsersDto {
  myUserid: string;
  users: ChatUser[]
}

export interface ChatUser {
  id: string;
  name: string;
  lastMessage: string;
  lastMessageDate: string
  unreadCount?: number;
}

@Injectable({
  providedIn: 'root'
})
export class ChatService {
  private hubConnection!: signalR.HubConnection;
  private connected = false;
  public messages$ = new BehaviorSubject<ChatMessage[]>([]);
  public chatUsers$ = new BehaviorSubject<ChatUser[]>([]);
  private myUserId: string = '';
  private activeChatUserId: string | null = null;
  private isChatWindowOpen = false;

  constructor(private http : HttpClient, private notification: NotificationService) {
    const url = environment.baseUrl + '/chatHub';

    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(url, { withCredentials: true })
      .withAutomaticReconnect()
      .build();

    // Старт соединения
    this.hubConnection
      .start()
      .then(() => {
        this.connected = true;
        console.log('SignalR connected');
      })
      .catch(err => console.error('SignalR connection error:', err));

    this.hubConnection.on("UpdateChatUsers", async () => {
      const data = await this.getChatUsers();
      this.chatUsers$.next(data.users);
    });

    // Слушаем входящие сообщения
    this.hubConnection.on('ReceiveMessage', (msgs: ChatMessage[]) => {
      this.messages$.next(msgs);

      if (this.activeChatUserId && this.isChatWindowOpen) {
        const last = msgs[msgs.length - 1];

        if (last && last.senderId === this.activeChatUserId) {
          this.markAsRead(this.activeChatUserId);

          //this.getChatUsers().then( x => this.chatUsers$.next(x.users) );
          
        }
      }
     });

    this.hubConnection.on("MessagesRead", (userId: string) => {

      if (!this.isChatWindowOpen) return;
      if (this.activeChatUserId !== userId) return;

      const updated = this.messages$.value.map(m => {
        if (m.senderId === this.myUserId && m.receiverId === userId) {
          return { ...m, readAt: new Date().toISOString() };
        }
        return m;
      });

      this.messages$.next(updated);
    });

  }

  setChatWindowState(isOpen: boolean) {
    this.isChatWindowOpen = isOpen;
  }

  async openChat(userId: string): Promise<ChatMessage[]> {
    await this.ensureConnected();

    this.activeChatUserId = userId;

    const history = await this.getChatHistory(userId);

    // сразу говорим серверу что сообщения прочитаны
    await this.markAsRead(userId);

    this.messages$.next(history);

    return history;
  }

  // Ждем подключения перед отправкой / получением данных
  private ensureConnected(): Promise<void> {
    if (this.connected) return Promise.resolve();
    return new Promise((resolve, reject) => {
      const check = () => {
        if (this.connected) resolve();
        else setTimeout(check, 50);
      };
      check();
    });
  }

  // Отправка сообщения
  async sendMessage(receiverId: string, message: string): Promise<void> {
    await this.ensureConnected();
    try {
      await this.hubConnection.invoke('SendMessage', receiverId, message);
    } catch (err) {
      const msg = getErrorMessage(err);
      this.notification.showError(msg);
      console.error('SendMessage error:', err);
    }
  }

  // Получение истории сообщений
  async getChatHistory(receiverId: string): Promise<ChatMessage[]> {
    await this.ensureConnected();
    try {
      return await this.hubConnection.invoke<ChatMessage[]>('GetChatHistory', receiverId);
    } catch (err) {
      console.error('GetChatHistory error:', err);
      return [];
    }
  }

  async getChatUsers(): Promise<ChatUsersDto> {
  await this.ensureConnected();
  try {
    const result = await this.hubConnection.invoke<ChatUsersDto>('GetChatUsers');

    // ⭐ ЗАПОМИНАЕМ МОЙ ID
    this.myUserId = result.myUserid;

    return result;
  } catch (err) {
    console.error('GetChatUsers error:', err);
    return { myUserid: '', users: [] };
  }
 }

  markAsRead(userId: string) {
    this.hubConnection.invoke("MarkAsRead", userId);
  }

  async deleteMessage(messageId: number, receiverId: string): Promise<void> {
    await this.ensureConnected();
    try {
      await this.hubConnection.invoke('DeleteMessage', messageId, receiverId);
    } catch (err) {
      console.error('DeleteMessage error:', err);
    }
  }
}