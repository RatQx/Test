import { Component, OnInit } from '@angular/core';
import {
  AbstractControl,
  FormBuilder,
  FormGroup,
  ValidationErrors,
  ValidatorFn,
  Validators,
} from '@angular/forms';
import { AuctionService } from '../../services/auction.service';
import { Auction } from '../../models/auction.model';
import { Subscription } from 'rxjs';
import { ActivatedRoute, Router } from '@angular/router';
import { formatTime } from '../../utils/time-formating';

@Component({
  selector: 'app-auction-form',
  templateUrl: './auction-form.component.html',
  styleUrl: './auction-form.component.scss',
})
export class AuctionFormComponent implements OnInit {
  populateFormSubscription: Subscription;
  public auctionForm!: FormGroup;
  public submitButton!: string;
  public auctions!: Auction;
  photoError = false;
  photoErrorMessage = '';
  photoSelected: boolean = false;
  constructor(
    private fb: FormBuilder,
    private auctionService: AuctionService,
    private activatedRoute: ActivatedRoute,
    private router: Router
  ) {
    this.populateFormSubscription = this.auctionService
      .sendPopulateForm()
      .subscribe((data) => {
        this.populateForm(data);
      });
  }
  ngOnInit(): void {
    this.activatedRoute.params.subscribe({
      next: (param) => {
        const id = param['id'];
        this.auctionService.getAuction(id).subscribe({
          next: (desq) => {
            this.auctions = desq;
            this.auctionForm.patchValue(desq);
            this.submitButton = 'Update Record';
          },
        });
      },
    });
    this.auctionForm = this.fb.group({
      auction_start_time: ['', [Validators.required]],
      auction_end_time: ['', [Validators.required]],
    });
    this.emptyForm();
  }
  onSubmit() {
    if (this.auctionForm.valid) {
      const auctionData = this.auctionForm.value as Auction;
      const auctionEndTime = this.auctionForm.get('auction_end_time')!.value;
      const formattedAuctionEndTime = formatTime(new Date(auctionEndTime));
      const auctionStartTime =
        this.auctionForm.get('auction_start_time')!.value;
      const formattedAuctionStartTime = formatTime(new Date(auctionStartTime));
      this.auctionForm.patchValue({
        auction_end_time: formattedAuctionEndTime,
        auction_start_time: formattedAuctionStartTime,
      });

      if (this.auctionForm.value.id > 0) {
        this.auctionService
          .updateAuction(this.auctionForm.getRawValue() as Auction)
          .subscribe((data) => {
            this.auctionService.updateList();
            this.emptyForm();
            this.router.navigateByUrl('auction');
          });
      } else {
        console.log(this.auctionForm.getRawValue() as Auction);
        this.auctionService
          .createAuction(this.auctionForm.getRawValue() as Auction)
          .subscribe((auction: any) => {
            console.log('new auction created');
            this.auctionService.updateList();
            this.emptyForm();
            this.router.navigateByUrl(`/auction-bid/${auction.id}`);
          }),
          (error: any) => {
            this.router.navigateByUrl('auction');
          };
      }
    }
  }
  get registerFormControl() {
    return this.auctionForm.controls;
  }
  emptyForm() {
    this.submitButton = 'Add Auction';
    this.auctionForm = this.fb.group(
      {
        PhotoPaths: ['', Validators.required],
        name: ['', Validators.required],
        country: ['', Validators.required],
        city: ['', Validators.required],
        starting_price: [
          '',
          [Validators.required, Validators.pattern('^[0-9]+([.][0-9]+)?$')],
        ],
        min_buy_price: [
          '',
          [Validators.required, Validators.pattern('^[0-9]+([.][0-9]+)?$')],
        ],
        buy_now_price: [
          '',
          [Validators.required, Validators.pattern('^[0-9]+([.][0-9]+)?$')],
        ],
        bid_ammount: [
          '',
          [Validators.required, Validators.pattern('^[0-9]*$')],
        ],
        auction_start_time: [
          '',
          [Validators.required, this.auctionStartTimeValidator()],
        ],

        auction_end_time: [
          '',
          [Validators.required, this.auctionEndTimeValidator()],
        ],
        category: ['', Validators.required],
        description: ['', Validators.required],
        item_build_year: [
          '',
          [Validators.required, Validators.pattern('^[0-9]*$')],
        ],
        item_mass: ['', [Validators.required, Validators.pattern('^[0-9]*$')]],
        condition: ['', Validators.required],
        material: ['', Validators.required],
      },
      {
        //validators: [this.buyNowValidator, this.minBuyPriceValidator],
      }
    );
  }
  populateForm(id: number) {
    this.submitButton = 'Update Record';
    this.auctionService.getAuction(id).subscribe((record) => {
      this.auctions = record as Auction;
      this.auctionForm = this.fb.group({
        id: this.auctions.id,
        PhotoPaths: this.auctions.photoPaths,
        name: this.auctions.name,
        country: this.auctions.country,
        city: this.auctions.city,
        starting_price: this.auctions.starting_price,
        bid_ammount: this.auctions.bid_ammount,
        min_buy_price: this.auctions.min_buy_price,
        auction_start_time: this.auctions.auction_start_time,
        auction_end_time: this.auctions.auction_end_time,
        buy_now_price: this.auctions.buy_now_price,
        category: this.auctions.category,
        description: this.auctions.description,
        item_build_year: this.auctions.item_build_year,
        item_mass: this.auctions.item_mass,
        condition: this.auctions.condition,
        material: this.auctions.material,
      });
    });
  }
  // buyNowValidator(formGroup: FormGroup) {
  //   const startingPrice = formGroup.get('starting_price');
  //   const buyNowPrice = formGroup.get('buy_now_price');

  //   if (
  //     startingPrice!.value &&
  //     buyNowPrice!.value &&
  //     parseFloat(startingPrice!.value) > parseFloat(buyNowPrice!.value)
  //   ) {
  //     buyNowPrice!.setErrors({ invalidPrice: true });
  //     return { invalidPrice: true };
  //   } else {
  //     buyNowPrice!.setErrors(null);
  //     return null;
  //   }
  // }
  // minBuyPriceValidator(formGroup: FormGroup) {
  //   const startingPrice = formGroup.get('starting_price');
  //   const minBuyPrice = formGroup.get('min_buy_price');

  //   if (
  //     startingPrice!.value &&
  //     minBuyPrice!.value &&
  //     parseFloat(startingPrice!.value) > parseFloat(minBuyPrice!.value)
  //   ) {
  //     minBuyPrice!.setErrors({ invalidPrice: true });
  //     return { invalidPrice: true };
  //   } else {
  //     minBuyPrice!.setErrors(null);
  //     return null;
  //   }
  // }
  auctionStartTimeValidator(): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      const auctionStartTime = new Date(control.value);
      const now = new Date();
      const minDate = new Date(now.getTime() - 60000);
      const maxDate = new Date(now.getTime() + 2592000000);

      if (auctionStartTime < minDate || auctionStartTime > maxDate) {
        return { invalidAuctionStartTime: true };
      } else {
        return null;
      }
    };
  }
  auctionEndTimeValidator(): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      const auctionEndTime = new Date(control.value);
      const auctionStartTime = new Date(
        this.auctionForm.get('auction_start_time')?.value
      );
      if (!auctionStartTime || isNaN(auctionStartTime.getTime())) {
        return { invalidAuctionEndTime: true };
      }
      const auctionStartTimeAdjusted = new Date(
        auctionStartTime.getTime() +
          (auctionEndTime.getTimezoneOffset() -
            auctionStartTime.getTimezoneOffset()) *
            60000
      );
      const minDate = new Date(auctionStartTimeAdjusted.getTime() + 86400000);
      const maxDate = new Date(auctionStartTimeAdjusted.getTime() + 2592000000);
      if (auctionEndTime < minDate || auctionEndTime > maxDate) {
        return { invalidAuctionEndTime: true };
      } else {
        return null;
      }
    };
  }
  onFileChange(event: any) {
    this.photoError = false;
    this.photoErrorMessage = '';

    if (event.target.files) {
      const allowedExtensions = ['jpg', 'jpeg', 'png', 'jfif'];
      const MAX_FILE_SIZE = 1024 * 1024; // 1 MB in bytes

      const isValid = Array.from(event.target.files).every((file: any) => {
        const isValidExtension = allowedExtensions.includes(
          file.name.split('.').pop().toLowerCase()
        );
        const isValidSize = file.size <= MAX_FILE_SIZE;
        if (!isValidExtension || !isValidSize) {
          this.photoError = true;

          if (!isValidExtension) {
            this.photoErrorMessage =
              'Please select only JPG, JPEG, JFIF, or PNG images.';
          } else if (!isValidSize) {
            this.photoErrorMessage =
              'File size exceeds the maximum limit of 1 MB.';
          }

          return false;
        }

        return true;
      });

      if (!isValid) {
        return;
      }

      for (let i = 0; i < event.target.files.length; i++) {
        const file = event.target.files[i];
        console.log('File: ' + file.name);
        this.uploadFile(file);
      }
      this.photoSelected = true;
      console.log('Photo(s) selected: ' + this.photoSelected);
    } else {
      this.photoSelected = false;
      console.log('Photo selected: ' + this.photoSelected);
    }
  }

  uploadFile(file: File) {
    const formData = new FormData();
    formData.append('files', file);
    this.auctionService.uploadPhoto(formData).subscribe({
      next: (paths: string[] | null) => {
        if (paths) {
          paths.forEach((photoPath: string) => {
            const currentPaths = this.auctionForm.get('PhotoPaths')!.value;
            this.auctionForm.patchValue({
              PhotoPaths: [...currentPaths, photoPath],
            });
          });
        }
      },
      error: (error) => {
        console.error('Error uploading file:', error);
      },
    });
  }
}
