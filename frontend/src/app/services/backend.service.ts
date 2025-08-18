import { HttpClient } from '@angular/common/http';
import { Injectable, OnInit } from '@angular/core';
import { Observable, tap } from 'rxjs';
import { ArticleData, CalendarData, Trigger, TriggerResult, TriggerTaskResult } from '../models/models';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class BackendService implements OnInit {

  //baseUrl = environment.apiUrl;
  private baseUrl = '/api';

  constructor(private httpClient: HttpClient) { }

  ngOnInit(): void {

  }

  getTwisters():Observable<Array<Array<string>>>{
    return this.httpClient.get<Array<Array<string>>>(`${this.baseUrl}/twisters`);
  }

  getExercises():Observable<Array<Array<string>>>{
    return this.httpClient.get<Array<Array<string>>>(`${this.baseUrl}/stretching`);
  }

  getRandomArticle():Observable<ArticleData>{
    return  this.httpClient.get<ArticleData>(`${this.baseUrl}/article/random`);
  }

  createTrigger(trigger:Trigger):Observable<TriggerResult>{
    return this.httpClient.post<TriggerResult>(`${this.baseUrl}/trigger/create`, trigger);
  }

  getTriggers():Observable<Array<TriggerResult>>{
    return this.httpClient.get<Array<TriggerResult>>(`${this.baseUrl}/trigger`);
  }

  deleteTrigger(triggerValue:string):Observable<any>{
    return this.httpClient.delete(`${this.baseUrl}/trigger/${triggerValue}`);
  }

  getTriggerTasks(triggerValue: string):Observable<Array<TriggerTaskResult>>{
    return this.httpClient.get<Array<TriggerTaskResult>>(`${this.baseUrl}/trigger/triggertasks/${triggerValue}`);
  }

  getCalendar(): Observable<CalendarData> {
    return this.httpClient.get<CalendarData>(`${this.baseUrl}/calendar/get`);
  }

  startLesson():Observable<any>{
    return this.httpClient.post<number>(`${this.baseUrl}/lesson/start`, null);
  }

  finishLesson(id: number, words: number, wps: number) {
  const body = { id, words, wps };

  
  return this.httpClient.put(`${this.baseUrl}/lesson/finish`, body);
}

rewardLesson(id: number, value: boolean) {
  const body = { id, value };
  return this.httpClient.put(`${this.baseUrl}/lesson/reward`, body);
}
}
