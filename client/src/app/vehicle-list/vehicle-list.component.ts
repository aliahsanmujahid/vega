import { Component, OnInit } from '@angular/core';
import { KeyValuePair, Vehicle } from '../models/vehicle';
import { VehicleService } from '../services/vehicle.service';
import { faSort } from '@fortawesome/free-solid-svg-icons';

@Component({
  selector: 'app-vehicle-list',
  templateUrl: './vehicle-list.component.html',
  styleUrls: ['./vehicle-list.component.css']
})
export class VehicleListComponent implements OnInit {
  private readonly PAGE_SIZE = 10;
  queryResult: any = {};
  // allveicles: any = [];
  faSort = faSort;
  makes: KeyValuePair[];
  query: any = {
    pageSize: this.PAGE_SIZE
  };
  columns = [
    { title: 'Id' },
    { title: 'Contact Name', key: 'contactName', isSortable: true },
    { title: 'Make', key: 'make', isSortable: true },
    { title: 'Model', key: 'model', isSortable: true },
    { }
  ];

  constructor(private vehicleService: VehicleService) { }

  ngOnInit(): void {
    this.vehicleService.getMakes()
    .subscribe((makes: KeyValuePair[]) => {
      this.makes = makes;
    });
    this.populateVehicles();
     
  }
  onFilterChange() {
    this.query.page = 1;
    this.query.pageSize = 10;
    this.populateVehicles();
    // var vehicles = this.allveicles;
    // if(this.filter.makeId){
    //   vehicles = vehicles.filter(v => v.model.id == this.filter.makeId);
    // }
    // this.veicles = vehicles;
  }
  private populateVehicles() {
    this.vehicleService.getVehicles(this.query)
    .subscribe(result => this.queryResult = result);
  }
  sortBy(columnName) {
    if (this.query.sortBy === columnName) {
      this.query.isSortAscending = !this.query.isSortAscending;
    } else {
      this.query.sortBy = columnName;
      this.query.isSortAscending = true;
    }
    this.populateVehicles();
  }
  resetFilter() {
    this.query = {
      page: 1,
      pageSize: this.PAGE_SIZE
    };
    this.onFilterChange();
  }
  onPageChange(page) {
    this.query.page = page;
    this.populateVehicles();
  }
}
