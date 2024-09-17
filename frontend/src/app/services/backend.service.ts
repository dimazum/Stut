import { HttpClient } from '@angular/common/http';
import { Injectable, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { ArticleData, Trigger, TriggerResult, TriggerTaskResult } from '../models/models';

@Injectable({
  providedIn: 'root'
})
export class BackendService implements OnInit {

  baseUrl = 'https://95.182.122.4:5000'//

  constructor(private httpClient: HttpClient) { }

  ngOnInit(): void {

  }

  getTwisters():Observable<Array<Array<string>>>{
    return this.httpClient.get<Array<Array<string>>>(`${this.baseUrl}/api/twisters`);
  }

  getExercises():Observable<Array<Array<string>>>{
    return this.httpClient.get<Array<Array<string>>>(`${this.baseUrl}/api/stretching`);
  }

  getRandomArticle():Observable<ArticleData>{
    return  this.httpClient.get<ArticleData>(`${this.baseUrl}/api/article/random`);
  }

  createTrigger(trigger:Trigger):Observable<TriggerResult>{
    return this.httpClient.post<TriggerResult>(`${this.baseUrl}/api/trigger/create`, trigger);
  }

  getTriggers():Observable<Array<TriggerResult>>{
    return this.httpClient.get<Array<TriggerResult>>(`${this.baseUrl}/api/trigger`);
  }

  deleteTrigger(triggerValue:string):Observable<any>{
    return this.httpClient.delete(`${this.baseUrl}/api/trigger/${triggerValue}`);
  }

  getTriggerTasks(triggerValue: string):Observable<Array<TriggerTaskResult>>{
    return this.httpClient.get<Array<TriggerTaskResult>>(`${this.baseUrl}/api/trigger/triggertasks/${triggerValue}`);
  }
}
