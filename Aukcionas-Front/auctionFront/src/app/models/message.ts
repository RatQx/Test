export interface Message {
  id: number;
  chatId: number;
  senderId: string;
  content: string;
  sentAt: Date;
  isRead: boolean;
  senderUsername: string;
}
