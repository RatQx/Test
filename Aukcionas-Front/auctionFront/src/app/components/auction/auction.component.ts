import { Component } from '@angular/core';
import { Auction } from '../../models/auction.model';
import { AuctionService } from '../../services/auction.service';
import { Router, ActivatedRoute } from '@angular/router';
import { MatDialog } from '@angular/material/dialog';
import { SingleAuctionComponent } from '../single-auction/single-auction.component';
import { PageEvent } from '@angular/material/paginator';
import { LoaderService } from '../../services/loader.service';

@Component({
  selector: 'app-auction',
  templateUrl: './auction.component.html',
  styleUrl: './auction.component.scss',
})
export class AuctionComponent {
  auctions: any[] = [];
  auctionForModal: any[] = [];
  auctionToEdit?: Auction;
  isImageLoading: boolean = true;
  pageSizeOptions: number[] = [2, 10, 25, 50];
  pageSize: number = 10;
  pageIndex: number = 0;
  imageToShow: Map<number, string | ArrayBuffer | null> = new Map();
  totalAuctions = 0;
  currentPage = 0;
  displayedColumns: string[] = [
    'id',
    'name',
    'country',
    'starting_price',
    'auction_end_time',
    'update',
  ];
  constructor(
    private auctionService: AuctionService,
    private router: Router,
    private dialog: MatDialog,
    private route: ActivatedRoute,
    private loaderService: LoaderService
  ) {}
  ngOnInit(): void {
    this.getData('');
    console.log(window.navigator.language);
    this.route.queryParams.subscribe((params) => {
      const searchName = params['name'] || '';
      this.getData(searchName);
    });
  }
  updateAuctionList(auction: Auction[]) {
    this.auctions = auction;
  }
  initNewAuction() {
    this.router.navigate(['create-auction']);
  }

  getData(name: string) {
    this.loaderService.setLoading(true);

    this.auctionService.getAuctions({ name }).subscribe(
      (result) => {
        this.auctions = result;
        this.totalAuctions = this.auctions.length;
        this.preloadImages();
        this.loaderService.setLoading(false);
      },
      (error) => {
        console.error('Error loading auctions:', error);
        this.loaderService.setLoading(false);
      }
    );
  }

  openDetailsModal(product: any): void {
    console.log(product);
    this.dialog.open(SingleAuctionComponent, {
      width: '400px',
      data: { product },
    });
  }
  preloadImages(): void {
    this.auctions.forEach((product) => {
      if (product.photoPaths && product.photoPaths.length > 0) {
        console.log(product.photoPaths[0]);
        this.loadImage(product.photoPaths[0], product.id);
      }
    });
  }
  onPageChange(event: PageEvent): void {
    this.currentPage = event.pageIndex;
    this.pageSize = event.pageSize;
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
}
