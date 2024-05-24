import {Injectable} from '@angular/core';
import {HttpClient, HttpParams} from "@angular/common/http"
import {environment} from "../../../environments/environment";
import {Timetable} from "./timetable.model";
import {TimetableFormComponent} from "../timetable-form/timetable-form.component";

@Injectable({
  providedIn: 'root'
})
export class TimetableService {

  url: string = environment.apiBaseUrl + '/SearchByGroup';
  list: Timetable[] = [];
  formData: Timetable = new Timetable();

  constructor(private http: HttpClient) {
  }


  refreshList(groupId: number) {
    this.http.get<any>(`${this.url}/${groupId}`).subscribe({
      next: res => {
        this.list = res as Timetable[];
      },
      error: err => console.log(err)
    })
  }

  getTimetable(groupId: number) {
    return this.http.get<any>(`${this.url}/${groupId}`);
  }
}
