import { NgModule } from '@angular/core';

import {
  MatFormFieldModule, MatGridListModule, MatInputModule, MatSelectModule, MatCardModule, MatDialogModule,
  MatOptionModule, MatButtonModule, MatCheckboxModule, MatTableModule, MatToolbarModule, MatProgressSpinnerModule,
  MatSidenavModule, MatIconModule, MatListModule, MatSnackBarModule, MatPaginatorModule, MatSortModule, MatDatepickerModule,
  MatNativeDateModule
} from '@angular/material';

import {CdkTableModule} from '@angular/cdk/table';


@NgModule({
  imports: [MatFormFieldModule, MatGridListModule, MatInputModule, MatSelectModule, MatCardModule, MatDialogModule,
    MatOptionModule,  MatButtonModule, MatCheckboxModule, MatTableModule, CdkTableModule, MatProgressSpinnerModule,
    MatToolbarModule, MatSidenavModule, MatIconModule, MatListModule, MatSnackBarModule, MatPaginatorModule, MatSortModule,
  MatDatepickerModule, MatNativeDateModule],

  exports: [MatFormFieldModule, MatGridListModule, MatInputModule, MatSelectModule, MatCardModule, MatDialogModule,
    MatOptionModule,  MatButtonModule, MatCheckboxModule, MatTableModule, CdkTableModule, MatProgressSpinnerModule,
     MatToolbarModule, MatSidenavModule, MatIconModule, MatListModule, MatSnackBarModule, MatPaginatorModule, MatSortModule,
    MatDatepickerModule, MatNativeDateModule]
})
export class MaterialModule {

}
