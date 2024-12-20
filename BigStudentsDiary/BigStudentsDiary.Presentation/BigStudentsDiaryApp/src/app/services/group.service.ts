import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class GroupService {
  private apiUrl = 'https://localhost:7049/api/Groups'; // Замените на ваш URL API

  constructor(private http: HttpClient) { }

  getAllGroups(): Observable<any[]> {
    return this.http.get<any[]>(this.apiUrl);
  }
}
