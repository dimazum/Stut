import { AfterViewChecked, Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { ChatMessage, ChatService, ChatUser } from '../services/chat.service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { BackendService } from '../services/backend.service';


@Component({
  selector: 'app-chat',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './chat.component.html',
  styleUrls: ['./chat.component.css']
})


export class ChatComponent implements OnInit, AfterViewChecked {
  messages: ChatMessage[] = [];
  users: ChatUser[] = [];
  message = '';
  myUserId = '';
  receiverId = '';
  otherUserName = '';
  hasMessages = false;

  viewMode: 'list' | 'chat' = 'list';

  public isOpen: boolean = false;

  @ViewChild('scrollMe') private myScrollContainer!: ElementRef;

  constructor(private chatService: ChatService, private backendService: BackendService) { }

  ngAfterViewChecked(): void {
    this.scrollToBottom();
  }

  ngOnInit() {
    this.chatService.messages$.subscribe(msgs => this.messages = msgs);
    this.chatService.chatUsers$.subscribe(users => {
      this.users = users;
      this.hasMessages = users.some(u => (u.unreadCount ?? 0) > 0);
    });

    this.getUsers();
  }


  private scrollToBottom(): void {
    try {
      this.myScrollContainer.nativeElement.scrollTop =
        this.myScrollContainer.nativeElement.scrollHeight;
    } catch (err) { }
  }

  send() {
    if (!this.message.trim()) return;
    this.chatService.sendMessage(this.receiverId, this.message);
    this.message = '';
  }

  async selectUser(user: ChatUser) {
    this.receiverId = user.id;
    this.otherUserName = user.name;
    this.viewMode = 'chat';

    this.chatService.setChatWindowState(true);

    await this.chatService.openChat(user.id);

    this.getUsers();
  }

  // async loadChat(userId: string) {
  //   const messages = await this.chatService.getChatHistory(userId);
  //   this.messages = messages;

  //   this.chatService.markAsRead(userId);
  // }

  public async onToggle() {
    this.isOpen = !this.isOpen;

    this.chatService.setChatWindowState(this.isOpen && this.viewMode === "chat");

    // if (this.isOpen && this.viewMode === "chat") {
    //  this.getUsers()
    // }
  }

  async goBack() {
    this.viewMode = 'list';
    this.chatService.setChatWindowState(false);
    this.getUsers();
  }

  async getUsers(){

     const chatusers = await this.chatService.getChatUsers();
      this.users = chatusers.users;
      this.myUserId = chatusers.myUserid;
      this.hasMessages = chatusers.users.some(u => (u.unreadCount ?? 0) > 0);
  }

  onSpecialAction(msg: ChatMessage) {
    this.backendService.beLearner(msg.senderId).subscribe()
  }


  deleteMessage(id: number) {
    this.chatService.deleteMessage(id, this.receiverId);
  }

  formatDialogDate(dateStr: string): string {
    if (!dateStr) return '';

    const date = new Date(dateStr);
    const now = new Date();

    const diffMs = now.getTime() - date.getTime();
    const diffDays = Math.floor(diffMs / (1000 * 60 * 60 * 24));

    // 🟢 Сегодня → время
    if (diffDays === 0) {
      return date.toLocaleTimeString([], {
        hour: '2-digit',
        minute: '2-digit'
      });
    }

    // 🟡 Вчера
    if (diffDays === 1) {
      return 'Yesterday';
    }

    // 🔵 До недели → день недели
    if (diffDays < 7) {
      return date.toLocaleDateString('en-US', {
        weekday: 'short'
      }); // Mon, Tue
    }

    // 🟣 До года → день + месяц
    const sameYear = date.getFullYear() === now.getFullYear();

    if (sameYear) {
      return date.toLocaleDateString('en-US', {
        day: '2-digit',
        month: 'short'
      }); // 12 Apr
    }

    // ⚫ Старше года
    return date.toLocaleDateString('en-US', {
      day: '2-digit',
      month: 'short',
      year: 'numeric'
    }); // 12 Apr 2024
  }
}