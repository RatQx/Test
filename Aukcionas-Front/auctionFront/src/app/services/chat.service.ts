import { Injectable } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { BehaviorSubject, catchError, Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Chat } from '../models/chat';
import { Message } from '../models/message';

@Injectable({
  providedIn: 'root',
})
export class ChatService {
  private baseApiUrl: string = environment.apiUrl + '/Chat';

  constructor(private http: HttpClient) {}

  getChats(): Observable<Chat[]> {
    return this.http.get<Chat[]>(`${this.baseApiUrl}/chats`);
  }

  getMessages(chatId: number): Observable<Message[]> {
    return this.http.get<Message[]>(
      `${this.baseApiUrl}/chat-messages/${chatId}`
    );
  }

  getAllUsers(): Observable<string[]> {
    return this.http.get<string[]>(`${this.baseApiUrl}/getUsers`);
  }

  addChat(receiver: string): Observable<any> {
    const headers = new HttpHeaders().set('Content-Type', 'application/json');
    return this.http
      .post<any>(`${this.baseApiUrl}/addChat`, JSON.stringify(receiver), {
        headers: headers,
      })
      .pipe(
        catchError((error: any) => {
          throw error;
        })
      );
  }

  addMessage(message: Message): Observable<Message> {
    return this.http
      .post<Message>(`${this.baseApiUrl}/addMessage`, message)
      .pipe(
        catchError((error: any) => {
          throw error;
        })
      );
  }
  deleteChat(chatId: number): Observable<void> {
    return this.http.delete<void>(`${this.baseApiUrl}/delete-chat/${chatId}`);
  }

  deleteMessage(messageId: number): Observable<void> {
    return this.http.delete<void>(
      `${this.baseApiUrl}/delete-message/${messageId}`
    );
  }
}
