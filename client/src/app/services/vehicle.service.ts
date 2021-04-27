import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class VehicleService {

  constructor(private http: HttpClient) { }

  baseUrl = 'https://localhost:5001/api/makes/';
  featuresUrl = 'https://localhost:5001/api/features/';
  createUrl = 'https://localhost:5001/api/vehicles/';
  
  getMakes() {
    return this.http.get(this.baseUrl);
  }
  getFeatures() {
    return this.http.get(this.featuresUrl);
  }
  create(vehicle) {
    return this.http.post(this.createUrl, vehicle);
  }
  getVehicle(id) {
    return this.http.get(this.createUrl  + id);
  }
  update(vehicle) {
    return this.http.put(this.createUrl + vehicle.id, vehicle);
  }
  delete(id) {
        return this.http.delete(this.createUrl  + id);
  }
  getVehicles(filter){
    //console.log(this.createUrl + '?' + this.toQueryString(filter));
    return this.http.get(this.createUrl + '?' + this.toQueryString(filter));
  }  
  toQueryString(obj) {
    const parts = [];
    for (const property in obj) {
      let value = obj[property];
      if (value != null && value !== undefined) {
        parts.push(encodeURIComponent(property) + '=' + encodeURIComponent(value));
      }
    }
    return parts.join('&');
  }
}
