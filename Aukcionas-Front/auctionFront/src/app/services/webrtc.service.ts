import { Injectable } from '@angular/core';
import { SignalRService } from './signalr.service';

@Injectable({
  providedIn: 'root',
})
export class WebRTCService {
  private peerConnection!: RTCPeerConnection;
  private localStream!: MediaStream;

  constructor() {}

  async startStreaming(localVideoElement: HTMLVideoElement) {
    try {
      this.localStream = await navigator.mediaDevices.getUserMedia({
        video: true,
        audio: true,
      });
      localVideoElement.srcObject = this.localStream;
      this.createPeerConnection();
    } catch (error) {
      console.error('Error accessing media devices:', error);
    }
  }

  private createPeerConnection() {
    this.peerConnection = new RTCPeerConnection();
    this.localStream.getTracks().forEach((track) => {
      this.peerConnection.addTrack(track, this.localStream);
    });
    // Add event listeners for signaling and handling remote stream
  }
}
