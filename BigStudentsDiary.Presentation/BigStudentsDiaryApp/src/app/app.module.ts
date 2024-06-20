import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';



import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { MatDialogModule } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatSelectModule } from '@angular/material/select';
import { MatTabsModule } from '@angular/material/tabs';


import { GroupService } from './services/group.service';







import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { TimetableComponent } from './timetable/timetable.component';
import { TimetableFormComponent } from './timetable/timetable-form/timetable-form.component';
import { HomeworkFormComponent } from './timetable/homework-form/homework-form.component';
import {HttpClient, HttpClientModule} from "@angular/common/http";
import {FormsModule, ReactiveFormsModule} from "@angular/forms";
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';
import { AuthModalComponent } from './auth-modal/auth-modal.component';
import {MatDialogActions, MatDialogContent} from "@angular/material/dialog";
import {MatTab, MatTabGroup} from "@angular/material/tabs";
import {MatFormField} from "@angular/material/form-field";
import {MatOption, MatSelect} from "@angular/material/select";

@NgModule({
  declarations: [
    AppComponent,
    TimetableComponent,
    TimetableFormComponent,
    HomeworkFormComponent,
    AuthModalComponent,

  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    FormsModule,
    MatDialogContent,
    MatTabGroup,
    ReactiveFormsModule,
    MatTab,
    MatFormField,
    MatSelect,
    MatOption,
    MatDialogActions,
    BrowserModule,
    HttpClientModule,
    ReactiveFormsModule

  ],
  providers: [
    provideAnimationsAsync()
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
