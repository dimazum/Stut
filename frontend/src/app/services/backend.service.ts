import { HttpClient } from '@angular/common/http';
import { Injectable, OnInit } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class BackendService implements OnInit {

  baseUrl = 'http://95.182.122.4:5000'

  constructor(private httpClient: HttpClient) { }

  ngOnInit(): void {

  }

  getTwisters():Observable<Array<Array<string>>>{
    return this.httpClient.get<Array<Array<string>>>(`${this.baseUrl}/api/twisters`);
  }

  getExercises():Observable<Array<Array<string>>>{
    return this.httpClient.get<Array<Array<string>>>(`${this.baseUrl}/api/stretching`);
  }

}
