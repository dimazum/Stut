import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { BehaviorSubject, Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { HttpClient } from '@angular/common/http';

export interface ChatMessage {
  id: number;
  senderId: string;
  receiverId: string;
  message: string;
  sentAt: string;
}

interface ChatUsersDto {
  myUserid: string;
  users: ChatUser[]
}

export interface ChatUser {
  id: string;
  name: string;
}

@Injectable({
  providedIn: 'root'
})
export class ChatService {
  private hubConnection!: signalR.HubConnection;
  private connected = false;
  public messages$ = new BehaviorSubject<ChatMessage[]>([]);

  constructor(private http : HttpClient) {
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

    // Слушаем входящие сообщения
    this.hubConnection.on('ReceiveMessage', (msgs: ChatMessage[]) => {
      this.messages$.next(msgs);
    });
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
      return await this.hubConnection.invoke<ChatUsersDto>('GetChatUsers');
    } catch (err) {
      console.error('GetChatUsers error:', err);
        return { myUserid: '', users: [] };
    }
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