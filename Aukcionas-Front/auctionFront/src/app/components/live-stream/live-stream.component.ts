import { Component, OnInit } from '@angular/core';
import { SignalRService } from '../../services/signalr.service';
import { LivestreamService } from '../../services/livestream.service';
import { UserService } from '../../services/user.service';
import { ActivatedRoute } from '@angular/router';
import { AuctionService } from '../../services/auction.service';
import { Auction } from '../../models/auction.model';
import { DomSanitizer, SafeResourceUrl } from '@angular/platform-browser';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
  selector: 'app-live-stream',
  templateUrl: './live-stream.component.html',
  styleUrls: ['./live-stream.component.scss'],
})
export class LiveStreamComponent implements OnInit {
  livestreamSrc: SafeResourceUrl | null = null;
  livestreamActive: boolean = false;
  userid!: '';
  auctionId?: number;
  auction: Auction | undefined;
  constructor(
    private livestreamService: LivestreamService,
    private userService: UserService,
    private route: ActivatedRoute,
    private auctionService: AuctionService,
    private sanitizer: DomSanitizer
  ) {}

  ngOnInit(): void {
    this.route.params.subscribe((params) => {
      console.log('params recived');
      this.auctionId = params['id'];
      this.userService.getUserInfo().subscribe((userInfo) => {
        this.userid = userInfo.userInfo.id;
      });
      if (this.auctionId) {
        this.auctionService.getAuction(this.auctionId).subscribe((auction) => {
          this.auction = auction;
          if (auction.savedUrl !== null) {
            this.livestreamSrc = this.sanitizer.bypassSecurityTrustResourceUrl(
              auction.savedUrl!
            );
            this.livestreamActive = true;
          } else {
            this.livestreamSrc = null;
          }
        });
      }
    });
  }

  startLivestream(): void {
    this.livestreamService.startLivestream(this.auctionId!).subscribe(
      (response: any) => {
        this.livestreamSrc = this.sanitizer.bypassSecurityTrustResourceUrl(
          response.livestreamUrl
        );
        this.livestreamActive = true;
        console.log(this.livestreamSrc);
      },
      (error) => console.error('Error starting livestream:', error)
    );
  }

  stopLivestream(): void {
    this.livestreamService.stopLivestream(this.auctionId!).subscribe(
      () => {
        this.livestreamSrc = null;
        this.livestreamActive = false;
        console.log('Livestream stopped successfully.');
      },
      (error) => {
        if (error.status === 204) {
          this.livestreamSrc = null;
          this.livestreamActive = false;
          console.log('Livestream stopped successfully.');
        } else {
          console.error('Error stopping livestream:', error);
        }
      }
    );
  }
}
