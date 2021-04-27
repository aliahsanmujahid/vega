import * as _ from 'underscore';
import { Component, OnInit } from '@angular/core';
import { VehicleService } from '../services/vehicle.service';
import { ToastrService } from 'ngx-toastr';
import { ActivatedRoute, Router } from '@angular/router';
import { forkJoin } from 'rxjs';
import { SaveVehicle, Vehicle } from '../models/vehicle';

@Component({
  selector: 'app-vehicle-form',
  templateUrl: './vehicle-form.component.html',
  styleUrls: ['./vehicle-form.component.css']
})
export class VehicleFormComponent implements OnInit {
  makes: any = [];
  models: any = [];
  features: any = [];
  vehicle: SaveVehicle = {
    id: 0,
    makeId: 0,
    modelId: 0,
    isRegistered: false,
    features: [],
    contact: {
      name: '',
      email: '',
      phone: '',
    }
  };
  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private vehicleService: VehicleService,
    private toastr: ToastrService) {
      route.params.subscribe(p => {
        this.vehicle.id = +p.id | 0;
      });
    }
  ngOnInit(): void {
    const sources = [
      this.vehicleService.getMakes(),
      this.vehicleService.getFeatures(),
    ];
    if (this.vehicle.id){
      sources.push(this.vehicleService.getVehicle(this.vehicle.id));
    }
    forkJoin(sources).subscribe(data => {
      this.makes = data[0];
      this.features = data[1];
      if (this.vehicle.id) {
        this.setVehicle(data[2]);
        this.populateModels();
      }
    }, err => {
      this.router.navigate(['']);
    });
  }

  private setVehicle(v) {
    this.vehicle.id = v.id;
    this.vehicle.makeId = v.make.id;
    this.vehicle.modelId = v.model.id;
    this.vehicle.isRegistered = v.isRegistered;
    this.vehicle.contact = v.contact;
    this.vehicle.features = _.pluck(v.features, 'id');
  }

  onMakeChange(){
      this.populateModels();
      delete this.vehicle.modelId;
  }

  private populateModels() {
      const selectedMake = this.makes.find(m => m.id == this.vehicle.makeId);
      this.models = selectedMake ? selectedMake.models : [];
  }

  onFeatureToggle(featureId, $event) {
    if ($event.target.checked){
      this.vehicle.features.push(featureId);
    }
    else {
      const index = this.vehicle.features.indexOf(featureId);
      this.vehicle.features.splice(index, 1);
    }
  }

  submit() {
    const result$ = (this.vehicle.id) ? this.vehicleService.update(this.vehicle) : this.vehicleService.create(this.vehicle);
    result$.subscribe(vehicle => {
        this.toastr.success('Form Submited', 'Toastr fun!');
    },
    error => {
      console.log(error);
    }
   );
  }
   delete(){
     this.vehicleService.delete(this.vehicle.id).subscribe( x => {
      this.router.navigate(['']);
     });
  
   }
}
