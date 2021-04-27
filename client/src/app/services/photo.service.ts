import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable()
export class PhotoService {

  constructor(private http: HttpClient) { }
  baseUrl = 'https://localhost:5001';
  upload(vehicleId, photo) {
    var formData = new FormData();
    formData.append('file', photo);
    return this.http.post(`${this.baseUrl }/api/vehicles/photos/${vehicleId}`, formData);
  }

  getPhotos(vehicleId) {
    return this.http.get(this.baseUrl + `/api/vehicles/photos/${vehicleId}`);
  }
}