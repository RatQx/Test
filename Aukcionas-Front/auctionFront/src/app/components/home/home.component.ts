import { Component, OnInit } from '@angular/core';
import { AuctionService } from '../../services/auction.service';
import { ActivatedRoute, Router } from '@angular/router';
import { MatDialog } from '@angular/material/dialog';
import { LoaderService } from '../../services/loader.service';
import { Auction } from '../../models/auction.model';
import { SingleAuctionComponent } from '../single-auction/single-auction.component';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss'],
})
export class HomeComponent implements OnInit {
  auctions: any[] = [];
  auctionForModal: any[] = [];
  auctionToEdit?: Auction;
  isImageLoading: boolean = true;
  imageToShow: Map<number, string | ArrayBuffer | null> = new Map();
  constructor(
    private auctionService: AuctionService,
    private router: Router,
    private dialog: MatDialog,
    private route: ActivatedRoute,
    private loaderService: LoaderService
  ) {}

  ngOnInit(): void {
    this.getData();
  }
  getData() {
    this.loaderService.setLoading(true);
    this.auctionService.getSuggestedAuctions().subscribe(
      (result) => {
        this.auctions = result;
        this.preloadImages();
        this.loaderService.setLoading(false);
      },
      (error) => {
        console.error('Error loading auctions:', error);
        this.loaderService.setLoading(false);
      }
    );
  }

  preloadImages(): void {
    this.auctions.forEach((product) => {
      if (product.photoPaths && product.photoPaths.length > 0) {
        console.log(product.photoPaths[0]);
        this.loadImage(product.photoPaths[0], product.id);
      }
    });
  }
  loadImage(imageFilename: string, auctionId: number): void {
    this.isImageLoading = true;
    this.auctionService.getImage(imageFilename).subscribe(
      (data) => {
        this.createImageFromBlob(data, auctionId);
        this.isImageLoading = false;
      },
      (error) => {
        this.isImageLoading = false;
        console.error('Error loading image:', error);
      }
    );
  }
  createImageFromBlob(image: Blob, auctionId: number): void {
    const reader = new FileReader();
    reader.addEventListener('load', () => {
      this.imageToShow.set(auctionId, reader.result);
    });
    if (image) {
      reader.readAsDataURL(image);
    }
  }
  openDetailsModal(product: any): void {
    console.log(product);
    this.dialog.open(SingleAuctionComponent, {
      width: '400px',
      data: { product },
    });
  }
  getContainerStyles(): { [key: string]: string } {
    const itemCount = this.auctions.length;
    const containerWidth = Math.min(100, itemCount * 20);
    return { 'width.%': containerWidth.toString() };
  }
}
