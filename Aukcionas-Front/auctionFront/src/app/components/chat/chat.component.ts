import { Component, OnInit } from '@angular/core';
import { ChatService } from '../../services/chat.service';
import { UserService } from '../../services/user.service';
import { Observable } from 'rxjs';
import { map, startWith, catchError, toArray, switchMap } from 'rxjs/operators';
import { FormControl } from '@angular/forms';
import { MatAutocompleteSelectedEvent } from '@angular/material/autocomplete';
import { Message } from '../../models/message';
import { Chat } from '../../models/chat';
import { concat, of } from 'rxjs';

@Component({
  selector: 'app-chat',
  templateUrl: './chat.component.html',
  styleUrls: ['./chat.component.scss'],
})
export class ChatComponent implements OnInit {
  chats$!: Observable<Chat[]>;
  selectedChatId: number | null = null;
  selectedChatUser: string | null = null;
  messages: any[] = [];
  username!: string;
  userId!: string;
  selectedUserId: string = '';
  showStartConversationDropdown: boolean = false;
  usernameControl = new FormControl();
  allUsernames!: string[];
  newMessageText: string = '';

  constructor(
    private chatService: ChatService,
    private userService: UserService
  ) {}

  ngOnInit(): void {
    this.userService.getUserInfo().subscribe((userInfo) => {
      this.username = userInfo.userInfo.userName;
      this.userId = userInfo.userInfo.id;
      this.loadChats();
      this.loadUsernames();
    });
  }

  loadChats(): void {
    this.chats$ = this.chatService.getChats();
  }
  openChat(chatId: number) {
    this.selectedChatId = chatId;
    this.loadMessages(chatId);
  }

  loadMessages(chatId: number): void {
    this.chatService.getMessages(chatId).subscribe((messages) => {
      this.messages = messages;
    });
  }

  loadUsernames() {
    this.chatService.getAllUsers().subscribe(
      (users) => {
        this.allUsernames = users;
      },
      (error) => {
        console.error('Error fetching usernames:', error);
      }
    );
  }

  displayUsername(username: string): string {
    return username ? username : '';
  }

  startConversation(): void {
    this.showStartConversationDropdown = true;
  }

  startChatWithUser(): void {
    if (!this.usernameControl.value) {
      return;
    }
    this.chatService
      .addChat(this.usernameControl.value)
      .pipe(
        catchError((error) => {
          console.error('Error creating chat:', error);
          return of(null);
        }),
        switchMap((newChat: Chat | null) => {
          if (newChat) {
            return concat(this.chats$, of([newChat]));
          } else {
            return this.chats$;
          }
        }),
        toArray()
      )
      .subscribe((updatedChats: (Chat | Chat[])[]) => {
        const chats: Chat[] = updatedChats.reduce(
          (acc: Chat[], item: Chat | Chat[]) => {
            if (Array.isArray(item)) {
              return acc.concat(item);
            } else {
              acc.push(item);
            }
            return acc;
          },
          []
        );
        this.chats$ = of(chats);
        this.cancelStartConversation();
      });
  }

  cancelStartConversation(): void {
    this.showStartConversationDropdown = false;
    this.selectedUserId = '';
    this.usernameControl.reset();
  }

  onUserSelected(event: MatAutocompleteSelectedEvent): void {
    this.selectedUserId = event.option?.value || '';
    console.log(this.selectedUserId);
  }
  sendMessage(): void {
    console.log(this.newMessageText.trim(), '  - ', this.selectedChatId);
    if (!this.newMessageText.trim() || !this.selectedChatId) {
      return;
    }
    const newMessage: Message = {
      id: 0,
      chatId: this.selectedChatId,
      senderId: this.userId,
      content: this.newMessageText.trim(),
      sentAt: new Date(),
      isRead: false,
      senderUsername: this.username,
    };
    this.chatService.addMessage(newMessage).subscribe(
      (message) => {
        this.messages.push(message);
        this.newMessageText = '';
      },
      (error) => {
        console.error('Error sending message:', error);
      }
    );
  }
  deleteChat(chatId: number): void {
    const confirmDelete = window.confirm(
      'Are you sure you want to delete this chat?'
    );
    if (confirmDelete) {
      this.chatService.deleteChat(chatId).subscribe(
        () => {
          this.chats$ = this.chats$.pipe(
            map((chats) => chats.filter((chat) => chat.id !== chatId))
          );
          this.messages = [];
        },
        (error) => {
          console.error('Error deleting chat:', error);
        }
      );
    }
  }
  deleteMessage(messageId: number): void {
    const confirmDelete = window.confirm(
      'Are you sure you want to delete this message?'
    );
    if (confirmDelete) {
      this.chatService.deleteMessage(messageId).subscribe(
        () => {
          this.messages = this.messages.filter(
            (message) => message.id !== messageId
          );
        },
        (error) => {
          console.error('Error deleting message:', error);
        }
      );
    }
  }
}
