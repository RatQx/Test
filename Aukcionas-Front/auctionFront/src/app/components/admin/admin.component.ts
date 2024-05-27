import { Component, OnInit, ViewChild } from '@angular/core';
import { AdminService } from '../../services/admin.service';
import { AuctionService } from '../../services/auction.service';
import { Router } from '@angular/router';
import { GroupedReport } from '../../models/grouped-report.model';
import { Report } from '../../models/report.model';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { FormControl } from '@angular/forms';
import { Observable } from 'rxjs';
import { map, startWith } from 'rxjs/operators';
import { Location } from '@angular/common';

@Component({
  selector: 'app-admin',
  templateUrl: './admin.component.html',
  styleUrls: ['./admin.component.scss'],
})
export class AdminComponent implements OnInit {
  @ViewChild(MatPaginator) paginator!: MatPaginator;
  showAllReportsTable = false;
  errorMessage: string = '';
  showGroupedReportsTable = false;
  groupedReports: GroupedReport[] = [];
  selectedAuctionIndex: number | null = null;
  selectedAuctionId: number | null = null;
  secondTableData: any[] = [];
  dataSource = new MatTableDataSource<any>();
  usernameControl = new FormControl();
  selectedUsername: string = '';
  allUsernames!: string[];
  filteredUsernames!: Observable<string[]>;
  totalReports = 0;
  paginatedAuctions: any[] = [];
  pageSize = 5;
  currentPage = 1;
  secondTotalReports = 0;
  secondPageSize = 5;
  secondCurrentPage = 1;
  selectedDeleteAuctionId: number | null = null;
  allAuctions: any[] = [];
  filteredAuctions!: Observable<any[]>;
  auctionControl = new FormControl();
  auctionsPageSize = 5;
  auctionsCurrentPage = 0;
  displayedColumns: string[] = [
    'id',
    'auction_Id',
    'userName',
    'report_Message',
    'report_Time',
    'remove',
  ];

  constructor(
    private adminService: AdminService,
    private auctionService: AuctionService,
    private router: Router,
    private location: Location
  ) {
    this.usernameControl = new FormControl();
  }
  ngOnInit() {
    this.loadUsernames();
    this.filteredUsernames = this.usernameControl.valueChanges.pipe(
      startWith(''),
      map((value) => this._filter(value))
    );
    this.loadAllAuctions();
  }
  loadAllAuctions() {
    this.adminService.getAllAuctions().subscribe(
      (auctions) => {
        this.allAuctions = auctions;
        this.onAuctionsPageChange({ pageIndex: this.auctionsCurrentPage, pageSize: this.auctionsPageSize } as PageEvent);
      },
      (error) => {
        console.error('Error fetching auctions:', error);
      }
    );
  }
  private _filterAuctions(value: string): any[] {
    const filterValue = value.toLowerCase();
    return this.allAuctions.filter((auction) =>
      auction.name.toLowerCase().includes(filterValue)
    );
  }
  loadUsernames() {
    this.adminService.getAllUsernames().subscribe(
      (usernames) => {
        this.allUsernames = usernames;
        this.filteredUsernames = this.usernameControl.valueChanges.pipe(
          startWith(''),
          map((value) => this._filter(value))
        );
      },
      (error) => {
        console.error('Error fetching usernames:', error);
      }
    );
  }

  private _filter(value: string): string[] {
    const filterValue = value.toLowerCase();
    if (this.allUsernames) {
      return this.allUsernames.filter((username) =>
        username.toLowerCase().includes(filterValue)
      );
    } else {
      return [];
    }
  }

  displayUsername(username: string): string {
    return username ? username : '';
  }
  ngAfterViewInit() {
    if (this.paginator) {
      this.dataSource.paginator = this.paginator;
    }
  }

  toggleAllReportsTable() {
    this.showAllReportsTable = !this.showAllReportsTable;
    if (this.showAllReportsTable) {
      this.loadReports(this.currentPage, this.pageSize);
    }
  }

  loadReports(page: number, pageSize: number): void {
    const startIndex = (page - 1) * pageSize;
    const endIndex = startIndex + pageSize;

    this.adminService.GetAllReports().subscribe((reports) => {
      this.totalReports = reports.length;
      this.dataSource.data = reports.slice(startIndex, endIndex);
    });
  }

  onPageChange(event: PageEvent): void {
    this.currentPage = event.pageIndex + 1;
    this.pageSize = event.pageSize;
    this.loadReports(this.currentPage, this.pageSize);
  }

  toggleGroupedReportsTable(): void {
    this.showGroupedReportsTable = !this.showGroupedReportsTable;
    if (this.showGroupedReportsTable) {
      this.loadGroupedReports(this.secondCurrentPage, this.secondPageSize);
    }
  }

  loadGroupedReports(page: number, pageSize: number): void {
    const startIndex = (page - 1) * pageSize;
    const endIndex = startIndex + pageSize;
    this.adminService.GetAllGroupedReports().subscribe((reports) => {
      const transformedReports = [];
      for (const auctionId in reports) {
        if (reports.hasOwnProperty(auctionId)) {
          const auctionReports = reports[auctionId];
          const reportCount = Array.isArray(auctionReports)
            ? auctionReports.length
            : 0;
          transformedReports.push({ auctionId: +auctionId, reportCount });
        }
      }
      this.groupedReports = transformedReports.slice(startIndex, endIndex);
    });
  }
  
  openAuction(auctionId: number): void {
    const relativeUrl = this.router.createUrlTree(['/auction-bid', auctionId]).toString();
    const fullUrl = `${window.location.origin}${this.location.prepareExternalUrl(relativeUrl)}`;
    window.open(fullUrl, '_blank');
    //window.open('https://google.com', '_blank');

  }

  openAuctionReports(auctionId: number, index: number): void {
    if (this.selectedAuctionIndex === index) {
      this.selectedAuctionIndex = null;
      this.secondTableData = [];
    } else {
      this.selectedAuctionId = auctionId;
      this.selectedAuctionIndex = index;
      this.adminService.GetAuctionReports(auctionId).subscribe((reports) => {
        this.secondTableData = reports;
      });
    }
  }

  closeAuctionReports(): void {
    this.selectedAuctionId = null;
    this.secondTableData = [];
  }

  onSecondPageChange(event: PageEvent): void {
    this.secondCurrentPage = event.pageIndex + 1;
    this.loadGroupedReports(this.secondCurrentPage, this.secondPageSize);
  }

  deleteReport(reportId: number) {
    this.adminService.DeleteReport(reportId).subscribe(
      () => {
        console.log('Report was deleted');
        if (this.showAllReportsTable) {
          const allReportsIndex = this.dataSource.data.findIndex(
            (report) => report.id === reportId
          );
          if (allReportsIndex !== -1) {
            this.dataSource.data.splice(allReportsIndex, 1);
            this.dataSource._updateChangeSubscription();
          }
        }

        if (this.showGroupedReportsTable) {
          const groupedIndex = this.groupedReports.findIndex(
            (report) => report.auctionId === this.selectedAuctionId
          );
          if (groupedIndex !== -1) {
            if (this.groupedReports[groupedIndex].reportCount > 0) {
              this.groupedReports[groupedIndex].reportCount--;
            }
            if (this.groupedReports[groupedIndex].reportCount === 0) {
              this.groupedReports.splice(groupedIndex, 1);
              this.selectedAuctionIndex = null;
              this.secondTableData = [];
            }
            const index = this.secondTableData.findIndex(
              (report) => report.id === reportId
            );
            if (index !== -1) {
              this.secondTableData.splice(index, 1);
            }
          }
        }
      },
      (error) => {
        console.error('Error deleting report:', error);
      }
    );
  }

  confirmDeleteUser(): void {
    if (!this.selectedUsername) {
      return;
    }

    if (
      confirm(
        `Are you sure you want to delete user "${this.selectedUsername}"?`
      )
    ) {
      this.adminService.deleteUserByUsername(this.selectedUsername).subscribe(
        () => {
          console.log(`User "${this.selectedUsername}" deleted successfully`);
          this.resetForm();
          this.loadUsernames();
        },
        (error: any) => {
          console.error('Error deleting user:', error);
        }
      );
    }
  }
  resetForm(): void {
    this.usernameControl.reset();
    this.selectedUsername = '';
  }
  confirmDeleteAuction(): void {
    if (!this.selectedDeleteAuctionId) {
      return;
    }

    if (
      confirm(
        `Are you sure you want to delete auction with ID ${this.selectedDeleteAuctionId}?`
      )
    ) {
      this.adminService
        .deleteAuctionById(this.selectedDeleteAuctionId)
        .subscribe(
          () => {
            console.log(
              `Auction with ID ${this.selectedDeleteAuctionId} deleted successfully`
            );
            this.selectedDeleteAuctionId = null;
            this.loadAllAuctions();
            this.errorMessage = '';
          },
          (error: any) => {
            if (error.status === 400) {
              this.errorMessage = error.error;
            } else {
              console.error('Error deleting auction:', error);
            }
          }
        );
    }
  }
  onAuctionSelectionChange(auctionId: number): void {
    this.selectedDeleteAuctionId = auctionId;
  }
  onAuctionsPageChange(event: PageEvent): void {
    const startIndex = event.pageIndex * event.pageSize;
    const endIndex = startIndex + event.pageSize;
    this.auctionsCurrentPage = event.pageIndex;
    this.paginatedAuctions = this.allAuctions.slice(startIndex, endIndex);
  }
}
