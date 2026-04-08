import { AfterViewChecked, Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { ChatMessage, ChatService, ChatUser } from '../services/chat.service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';


@Component({
  selector: 'app-chat',
  standalone: true,
  imports:[CommonModule, FormsModule],
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

  public isOpen: boolean = false;

  @ViewChild('scrollMe') private myScrollContainer!: ElementRef;

  constructor(private chatService: ChatService) {}

  ngAfterViewChecked(): void {
    this.scrollToBottom();
  }

  ngOnInit() {
    this.chatService.messages$.subscribe(msgs => this.messages = msgs);
  }


  private scrollToBottom(): void {
    try {
      this.myScrollContainer.nativeElement.scrollTop =
        this.myScrollContainer.nativeElement.scrollHeight;
    } catch (err) {}
  }

  send() {
    if (!this.message.trim()) return;
    this.chatService.sendMessage(this.receiverId, this.message);
    this.message = '';
  }

  selectUser(user: ChatUser) {
    this.receiverId = user.id;
    this.otherUserName = this.users.find(x=> x.id == user.id)?.name!
    this.loadChat(user.id);
  }

  async loadChat(userId: string) {
    const history = await this.chatService.getChatHistory(userId);
    this.messages = history;
  }

  public async onToggle() {
    this.isOpen = !this.isOpen;

    if(this.isOpen){
      const chatusers = await this.chatService.getChatUsers();
      this.users = chatusers.users;
      this.myUserId = chatusers.myUserid;

      if(this.receiverId !== ''){
        this.loadChat(this.receiverId);
        return;
      }

      if(chatusers.users.length > 0){
        this.receiverId = chatusers.users[0].id;
        this.otherUserName = chatusers.users[0].name;
        this.loadChat(chatusers.users[0].id);
      }
    }
  }

  deleteMessage(id: number){
    this.chatService.deleteMessage(id, this.receiverId);
  }
}