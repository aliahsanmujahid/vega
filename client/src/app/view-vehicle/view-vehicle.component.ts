import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { PhotoService } from '../services/photo.service';
import { VehicleService } from '../services/vehicle.service';

@Component({
  selector: 'app-view-vehicle',
  templateUrl: './view-vehicle.component.html',
  styleUrls: ['./view-vehicle.component.css']
})
export class ViewVehicleComponent implements OnInit {
  @ViewChild('fileInput') fileInput: ElementRef;
  vehicle: any;
  photos: any = [];
  vehicleId: number; 
  constructor(
    private route: ActivatedRoute, 
    private router: Router,
    private toasty: ToastrService,
    private photoService: PhotoService,
    private vehicleService: VehicleService,
    private toastr: ToastrService) { 
    route.params.subscribe(p => {
      this.vehicleId = +p['id'];
      if (isNaN(this.vehicleId) || this.vehicleId <= 0) {
        router.navigate(['/vehicles']);
        return; 
      }
    });
  }

  ngOnInit(): void {
    this.photoService.getPhotos(this.vehicleId) 
    .subscribe(photo =>{
      this.photos = photo
      console.log(this.photos);
    } );
    this.vehicleService.getVehicle(this.vehicleId)
    .subscribe(
      v => this.vehicle = v,
      err => {
        if (err.status == 404) {
          this.router.navigate(['/vehicles']);
          return; 
        }
      });
  }

  uploadPhoto() {    
    var nativeElement: HTMLInputElement = this.fileInput.nativeElement;
    var file = nativeElement.files[0];
    nativeElement.value = ''; 
    this.photoService.upload(this.vehicleId, file)
      .subscribe(photo => {
        console.log(photo);
        this.photos.push(photo);
        this.toastr.success('Submited', 'fun!');
      },
      err => {
        this.toastr.error('Not Submited', 'Not fun!');
      });
      
  }
  public createImgPath = (photo) => {
     return `https://localhost:5001/uploads/${ photo.fileName}`;
  }

}
