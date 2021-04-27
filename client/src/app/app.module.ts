import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { RouterModule, Routes } from '@angular/router';
import { HttpClientModule } from '@angular/common/http';
import { AppComponent } from './app.component';
import { VehicleFormComponent } from './vehicle-form/vehicle-form.component';
import { NavComponent } from './nav/nav.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { VehicleService } from './services/vehicle.service';
import { FormsModule } from '@angular/forms';
import { ToastrModule } from 'ngx-toastr';
import { VehicleListComponent } from './vehicle-list/vehicle-list.component';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { PaginationComponent } from './shared/pagination.component';
import { ViewVehicleComponent } from './view-vehicle/view-vehicle.component';
import { PhotoService } from './services/photo.service';

const routes: Routes = [
  { path: '', component: NavComponent},
  { path: 'vehicles', component: VehicleListComponent},
  { path: 'vehicles/new', component: VehicleFormComponent},
  { path: 'vehicles/edit/:id', component: VehicleFormComponent},
  { path: 'vehicles/:id', component: ViewVehicleComponent },
];

@NgModule({
  declarations: [
    AppComponent,
    VehicleFormComponent,
    NavComponent,
    VehicleListComponent,
    PaginationComponent,
    ViewVehicleComponent,
  ],
  imports: [
    FormsModule,
    BrowserModule,
    HttpClientModule,
    RouterModule.forRoot(routes),
    BrowserAnimationsModule,
    ToastrModule.forRoot(),
    FontAwesomeModule,
  ],
  exports: [
    RouterModule
  ],
  providers: [
    VehicleService,
    PhotoService
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
