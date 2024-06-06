import {
  AfterViewInit,
  ChangeDetectionStrategy,
  Component,
  ElementRef,
  Inject,
  OnInit,
  ViewChild,
} from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Auction } from '../../models/auction.model';
import { AuctionService } from '../../services/auction.service';
import { UserService } from '../../services/user.service';
import { CommentService } from '../../services/comment.service';
import { calculateTimeDifference } from '../../utils/calculate-time-left';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Comment } from '../../models/comment.model';
import { MatDialog } from '@angular/material/dialog';
import { ReportModalComponent } from '../report-modal/report-modal.component';
import { ReportService } from '../../services/report.service';
import { BidService } from '../../services/bid.service';
import { Subscription } from 'rxjs';
import { SignalRService } from './../../services/signalr.service';
import { Payment } from '../../models/payment.model';
import { PaymentService } from '../../services/payment.service';
import { ChangeDetectorRef } from '@angular/core';
import { DOCUMENT } from '@angular/common';
import { ProfileCheckComponent } from '../profile-check/profile-check.component';
import { AdminService } from '../../services/admin.service';

@Component({
  selector: 'app-auction-bid',
  templateUrl: './auction-bid.component.html',
  styleUrls: ['./auction-bid.component.scss'],
})
export class AuctionBidComponent implements OnInit, AfterViewInit {
  auctionId?: number;
  auction: Auction | undefined;
  public timeDiff: string | undefined;
  public isAuctionLiked: boolean = false;
  comments: Comment[] = [];
  commentForm!: FormGroup;
  isAuthenticated!: boolean;
  isAdmin!: boolean;
  username!: '';
  userid!: '';
  canBuyNow = true;
  canBidNow = true;
  slideIndex: number = 0;
  private bidSubscription!: Subscription;
  commentAdditionInProgress = false;
  bidButtonDisabled: boolean = false;
  likeButtonDisabled: boolean = false;

  constructor(
    private route: ActivatedRoute,
    private auctionService: AuctionService,
    private userService: UserService,
    private adminService: AdminService,
    private commentService: CommentService,
    private fb: FormBuilder,
    private dialog: MatDialog,
    private reportService: ReportService,
    private bidService: BidService,
    private signalRService: SignalRService,
    private paymentService: PaymentService,
    private changeDetectorRef: ChangeDetectorRef,
    @Inject(DOCUMENT) private document: Document,
    private router: Router
  ) {
    this.userService.isAuthenticated$.subscribe((isAuthenticated) => {
      this.isAuthenticated = isAuthenticated;
    });
    this.userService.isAdmin$.subscribe((isAdmin) => {
      this.isAdmin = isAdmin;
      console.log('is user admin ', this.isAdmin);
    });
  }

  paidFor = false;
  ngOnInit(): void {
    this.bidService.createBidConnection();
    this.signalRService.startConnection();
    this.bidService.getNewBidObservable().subscribe((bid) => {
      this.handleNewBid(bid.auctionId, bid.bidAmount, bid.username);
    });
    this.route.params.subscribe((params) => {
      console.log('params recived');
      this.auctionId = params['id'];
      this.getAuctionDetails();
      this.userService.getUserInfo().subscribe((userInfo) => {
        this.username = userInfo.userInfo.userName;
        this.userid = userInfo.userInfo.id;
        console.log(this.username);
        console.log(this.userid);
      });
    });
    this.commentForm = this.fb.group({
      text: ['', Validators.required],
    });
    setInterval(() => {
      this.timeDiff = this.calculateTimeDifference();
    }, 1000);
  }
  ngAfterViewInit(): void {}

  ngOnDestroy(): void {
    this.bidService.stopBidConnection();
  }

  handleNewBid(auctionId: number, bidAmount: number, username: string) {
    if (this.auction && this.auction.id === auctionId) {
      this.auction.auction_biders_list!.push(username);
      this.auction.bidding_amount_history!.push(bidAmount);
      this.auction.bidding_times_history!.push(new Date());
      setTimeout(() => {
        this.shouldDisplayBidNowButton();
        this.shouldDisplayBuyNowButton();
      }, 100);
    }
  }

  calculateTimeDifference(): string {
    if (
      this.auction &&
      this.auction.auction_end_time &&
      this.auction.auction_start_time
    ) {
      let auctionEndTime: Date;
      let auctionStartTime: Date;
      if (typeof this.auction.auction_end_time === 'string') {
        auctionEndTime = new Date(this.auction.auction_end_time);
      } else if (this.auction.auction_end_time instanceof Date) {
        auctionEndTime = this.auction.auction_end_time;
      } else {
        return 'Invalid date';
      }
      if (typeof this.auction.auction_start_time === 'string') {
        auctionStartTime = new Date(this.auction.auction_start_time);
      } else if (this.auction.auction_start_time instanceof Date) {
        auctionStartTime = this.auction.auction_start_time;
      } else {
        return 'Invalid date';
      }

      return calculateTimeDifference(
        auctionStartTime.toString(),
        auctionEndTime.toString()
      );
    }

    return '';
  }
  getAuctionDetails(): void {
    if (this.auctionId) {
      this.auctionService.getAuction(this.auctionId).subscribe((auction) => {
        this.auction = auction;
        console.log('loading all');
        this.getComments();
        this.shouldDisplayBuyNowButton();
        this.shouldDisplayBidNowButton();
        this.checkIfAuctionIsLiked();
        this.preloadImages();
        this.showSlides(this.slideIndex);
      });
    }
  }

  checkIfAuctionIsLiked(): void {
    this.userService.getUserInfo().subscribe((userInfo) => {
      if (
        userInfo.userInfo.id &&
        this.auctionId &&
        this.auction?.auction_likes_list
      ) {
        if (this.auction?.auction_likes_list.includes(userInfo.userInfo.id)) {
          this.isAuctionLiked = true;
        }
      }
    });
  }
  likeOrUnlikeAuction(): void {
    const auctionId = this.auctionId;
    if (auctionId) {
      this.likeButtonDisabled = true;
      this.isAuctionLiked = !this.isAuctionLiked;
      this.changeDetectorRef.detectChanges();
      setTimeout(() => {
        this.likeButtonDisabled = false;
        this.changeDetectorRef.detectChanges();
        if (this.isAuctionLiked) {
          this.auction!.auction_likes!++;
          this.auctionService.likeAuction(auctionId).subscribe(
            (response) => {
              console.log('Auction liked successfully');
              console.log('Response', response);
            },
            (error) => {
              console.error('Error liking auction:', error);
              this.isAuctionLiked = false;
              this.auction!.auction_likes!--;
              this.changeDetectorRef.detectChanges();
            }
          );
        } else {
          this.auction!.auction_likes!--;
          this.auctionService.unlikeAuction(auctionId).subscribe(
            (response) => {
              console.log('Auction unliked successfully');
              console.log('Response', response);
            },
            (error) => {
              console.error('Error unliking auction:', error);
              this.isAuctionLiked = true;
              this.auction!.auction_likes!++;
              this.changeDetectorRef.detectChanges();
            }
          );
        }
      }, 2000);
    }
  }
  getComments(): void {
    if (this.auctionId) {
      this.commentService.getCommentsForAuction(this.auctionId).subscribe(
        (comments) => {
          this.comments = comments;
        },
        (error) => {
          console.error('Error fetching comments:', error);
        }
      );
    }
  }
  addComment(): void {
    this.commentAdditionInProgress = true;
    const newComment: Comment = {
      id: 0,
      date: new Date(),
      username: this.username,
      text: this.commentForm.value.text,
    };
    this.comments.push(newComment);
    this.commentForm.reset();
    this.commentService.addComment(this.auctionId!, newComment).subscribe(
      (comment) => {
        const index = this.comments.findIndex((c) => c.id === 0);
        if (index !== -1) {
          this.comments[index] = comment;
        }
        this.commentAdditionInProgress = false;
      },
      (error) => {
        const index = this.comments.findIndex((c) => c.id === 0);
        if (index !== -1) {
          this.comments.splice(index, 1);
        }
        console.error('Error adding comment:', error);
        this.commentAdditionInProgress = false;
      }
    );
  }
  calculateNextBid(): number {
    if (this.auction) {
      if (
        this.auction.bidding_amount_history &&
        this.auction.bidding_amount_history.length > 0
      ) {
        const lastBidAmount =
          this.auction.bidding_amount_history[
            this.auction.bidding_amount_history.length - 1
          ];

        if (typeof lastBidAmount === 'number') {
          const nextBidAmount = lastBidAmount + this.auction.bid_ammount!;
          return nextBidAmount;
        }
      }
      return this.auction.starting_price!;
    }
    return 0;
  }
  PlaceBid(auctionId: number, bidAmount: number) {
    this.bidButtonDisabled = true;
    this.bidService.placeBid(auctionId, bidAmount, this.username);
    setTimeout(() => {
      this.bidButtonDisabled = false;
      this.shouldDisplayBuyNowButton();
      this.shouldDisplayBidNowButton();
    }, 2000);
  }
  openReportModal(auctionId: number) {
    const dialogRef = this.dialog.open(ReportModalComponent, {
      data: { auctionId: auctionId },
    });
    dialogRef.afterClosed().subscribe((reportData) => {
      if (reportData) {
        this.reportService.CreateReport(reportData).subscribe(
          (response) => {
            console.log('Report submitted successfully');
            dialogRef.close();
          },
          (error) => {
            console.error('Error submitting report:', error);
            dialogRef.close();
          }
        );
      }
    });
  }

  reverseIndexArray(arr: any[]): number[] {
    return Array.from({ length: arr.length }, (_, i) => i).reverse();
  }

  nextSlide(): void {
    if (this.auction && this.slideIndex < this.auction.photoPaths!.length - 1) {
      this.slideIndex++;
    }
  }

  prevSlide(): void {
    if (this.slideIndex > 0) {
      this.slideIndex--;
    }
  }
  currentSlide(n: number): void {
    this.showSlides((this.slideIndex = n));
  }

  showSlides(n: number): void {
    let i;
    const slides = document.getElementsByClassName(
      'mySlides'
    ) as HTMLCollectionOf<HTMLElement>;
    const dots = document.getElementsByClassName(
      'dot'
    ) as HTMLCollectionOf<HTMLElement>;

    if (n >= slides.length) {
      this.slideIndex = 0;
    }
    if (n < 0) {
      this.slideIndex = slides.length - 1;
    }

    for (i = 0; i < slides.length; i++) {
      slides[i].style.display = 'none';
    }
    for (i = 0; i < dots.length; i++) {
      dots[i].className = dots[i].className.replace(' active', '');
    }

    slides[this.slideIndex].style.display = 'block';
    dots[this.slideIndex].className += ' active';
  }
  preloadImages(): void {
    if (
      this.auction &&
      this.auction.photoPaths &&
      this.auction.photoPaths.length > 0
    ) {
      this.auction.photoPaths.forEach((photoPath, index) => {
        this.loadImage(photoPath, index);
      });
    }
  }

  loadImage(imageFilename: string, index: number): void {
    this.auctionService.getImage(imageFilename).subscribe(
      (data) => {
        this.createImageFromBlob(data, index);
      },
      (error) => {
        console.error('Error loading image:', error);
      }
    );
  }

  createImageFromBlob(image: Blob, index: number): void {
    const reader = new FileReader();
    reader.addEventListener('load', () => {
      if (this.auction && this.auction.photoPaths) {
        this.auction.photoPaths[index] = reader.result as string;
      }
    });
    if (image) {
      reader.readAsDataURL(image);
    }
  }
  deleteComment(commentId: number): void {
    if (this.commentAdditionInProgress) {
      console.warn('Cannot delete comment right now.');
      return;
    }
    const index = this.comments.findIndex((c) => c.id === commentId);
    if (index !== -1) {
      const deletedComment = this.comments[index];
      this.comments.splice(index, 1);
      this.auctionService.deleteComment(this.auctionId!, commentId).subscribe(
        () => {
          console.log('Comment deleted successfully');
          this.getAuctionDetails();
        },
        (error) => {
          this.comments.splice(index, 0, deletedComment);
          console.error('Error deleting comment:', error);
        }
      );
    }
  }
  BuyNow(auctionId: number) {
    if (!this.isAuthenticated) {
      return;
    }
    if (!this.auction || this.auction.auction_ended || this.paidFor) {
      return;
    }
    if (!this.shouldDisplayBuyNowButton()) {
      return;
    }
    if (
      this.auction.bidding_amount_history![
        this.auction.bidding_amount_history!.length - 1
      ] >= this.auction.buy_now_price!
    ) {
      return;
    }

    this.paymentService.processPayment(auctionId).subscribe(
      (response) => {
        console.log(response.paymentUrl);
        console.log(response);
        window.open(response.paymentUrl, '_blank');
      },
      (error) => {
        console.error('Error processing payment:', error);
      }
    );
  }
  datesCheck(
    startTime: string | Date | undefined,
    endTime: string | Date | undefined
  ): boolean {
    if (!startTime || !endTime) {
      return false;
    }
    const auctionStartTime = new Date(startTime);
    const auctionEndTime = new Date(endTime);
    const currentTime = new Date();
    return auctionStartTime < currentTime && currentTime < auctionEndTime;
  }
  shouldDisplayBuyNowButton(): boolean {
    if (!this.auction || this.auction.auction_ended) {
      console.log('BuyNow -1');
      this.canBuyNow = false;
      return false;
    }
    if (
      !this.datesCheck(
        this.auction.auction_start_time,
        this.auction.auction_end_time
      )
    ) {
      console.log('BuyNow -2');
      this.canBuyNow = false;
      return false;
    }
    const lastBidAmount =
      this.auction.bidding_amount_history![
        this.auction.bidding_amount_history!.length - 1
      ];

    if (lastBidAmount > this.auction.buy_now_price!) {
      this.canBuyNow = false;
      console.log('BuyNow -3');
    }
    if (this.auction.username == this.userid) {
      this.canBuyNow = false;
      console.log('BuyNow -4');
    }
    if (!this.isAuthenticated) {
      this.canBuyNow = false;
      console.log('BuyNow -5');
    }
    return this.canBuyNow;
  }
  openProfileModal(username: string): void {
    console.log(username);
    this.dialog.open(ProfileCheckComponent, {
      width: '400px',
      data: { username },
    });
  }
  shouldDisplayBidNowButton(): boolean {
    console.log('Entering shouldDisplayBidNowButton');

    if (
      !this.auction ||
      this.auction.auction_ended ||
      !this.isAuthenticated ||
      this.auction.username === this.userid
    ) {
      console.log('Condition 1');
      this.canBidNow = false;
      return false;
    }
    if (
      !this.datesCheck(
        this.auction.auction_start_time,
        this.auction.auction_end_time
      )
    ) {
      this.canBidNow = false;
      return false;
    }
    if (
      this.auction.auction_biders_list &&
      this.auction.auction_biders_list.length > 0
    ) {
      const lastBidder =
        this.auction.auction_biders_list[
          this.auction.auction_biders_list.length - 1
        ];

      console.log('Last Bidder:', lastBidder);
      console.log('Current User:', this.username);
      if (lastBidder === this.username) {
        console.log('Condition 2');
        this.canBidNow = false;
        return false;
      }
    }

    console.log('Setting canBidNow to true');
    this.canBidNow = true;
    return true;
  }

  public endauction() {
    if (window.confirm('Are you sure you want to end this auction?')) {
      this.adminService.endauction(this.auctionId!).subscribe(() => {
        console.log('Auction ended successfully');
        this.auction!.auction_end_time = new Date();
        this.router.navigate(['/auction-bid', this.auctionId]);
      });
    } else {
      console.log('Auction end action cancelled');
    }
  }
}
