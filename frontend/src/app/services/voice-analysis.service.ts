import { Injectable, OnDestroy } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { BehaviorSubject, Observable } from 'rxjs';
import { VoiceAnalysisUpdateDto } from '../models/models';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class VoiceAnalysisService implements OnDestroy {
  private hubConnection: signalR.HubConnection;

  private voiceAnalysisSubject = new BehaviorSubject<VoiceAnalysisUpdateDto | null>(null);

  public voiceAnalysis$: Observable<VoiceAnalysisUpdateDto | null> = this.voiceAnalysisSubject.asObservable();

  constructor() {

    const url = environment.baseUrl + '/voice-analysis' ;
   this.hubConnection = new signalR.HubConnectionBuilder()
  .withUrl(url, {
    accessTokenFactory: () => {
      return localStorage.getItem('token') || '';
    }
  })
  .withAutomaticReconnect()
  .build();

    this.hubConnection.start()
      .then(() => console.log('Connected to VoiceAnalysisHub'))
      .catch((err) => console.error(err));

    this.hubConnection.on('UpdateVoiceAnalysis', (data: VoiceAnalysisUpdateDto) => {
      this.voiceAnalysisSubject.next(data);
    });
  }

  ngOnDestroy(): void {
    this.hubConnection.stop().catch(err => console.error(err));
    this.voiceAnalysisSubject.complete();
  }
}
