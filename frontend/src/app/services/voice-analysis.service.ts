import { Injectable, OnDestroy } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { BehaviorSubject, Observable, Subscription } from 'rxjs';
import { VoiceAnalysisUpdateDto } from '../models/models';
import { environment } from '../../environments/environment';
import { AuthService } from './auth.service';

@Injectable({
  providedIn: 'root'
})
export class VoiceAnalysisService implements OnDestroy {

  private hubConnection?: signalR.HubConnection;

  private authSubscription: Subscription;

  private voiceAnalysisSubject = new BehaviorSubject<VoiceAnalysisUpdateDto | null>(null);
  public voiceAnalysis$: Observable<VoiceAnalysisUpdateDto | null> =
    this.voiceAnalysisSubject.asObservable();

  // ===============================
  // BUFFER LOGIC
  // ===============================

  private bufferText = '';
  private sendInterval: any;

  private readonly SEND_INTERVAL = 10000; // 10 sec

  constructor(private authService: AuthService) {

    this.authSubscription = this.authService.userinfo$.subscribe(user => {

      if (user?.logged_in) {
        this.startConnection();
      } else {
        this.stopConnection();
      }

    });

  }

  // =====================================================
  // SIGNALR CONNECTION
  // =====================================================

  private async startConnection(): Promise<void> {

    if (this.hubConnection) return;

    const url = environment.baseUrl + '/voice-analysis';

    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(url, {
        accessTokenFactory: () =>
          localStorage.getItem('jwt-token') || ''
      })
      .withAutomaticReconnect()
      .build();

    // Получение результата анализа
    this.hubConnection.on('UpdateVoiceAnalysis', (data: VoiceAnalysisUpdateDto) => {
      this.voiceAnalysisSubject.next(data);
    });

    try {
      await this.hubConnection.start();
      console.log('✅ Connected to VoiceAnalysisHub');

      this.startIntervalSender();

    } catch (err) {
      console.error('❌ SignalR connection error:', err);
    }
  }

  // =====================================================
  // SEND LOGIC
  // =====================================================

  public analyzeVoice(text: string): void {

    if (!this.hubConnection) {
      console.warn('SignalR not connected');
      return;
    }

    this.bufferText += text;

  }

  /**
   * Interval sender запускается только 1 раз
   */
  private startIntervalSender(): void {

    if (this.sendInterval) return;

    this.sendInterval = setInterval(async () => {

      if (!this.bufferText || !this.hubConnection) return;

      const textToSend = this.bufferText;
      this.bufferText = '';

      try {
        await this.hubConnection.invoke('AnalyzeVoice', textToSend);
      } catch (err) {
        console.error('Send buffer error:', err);
      }

    }, this.SEND_INTERVAL);

  }

  // =====================================================
  // DISCONNECT
  // =====================================================

  private async stopConnection(): Promise<void> {

    if (this.sendInterval) {
      clearInterval(this.sendInterval);
      this.sendInterval = null;
    }

    this.bufferText = '';

    if (!this.hubConnection) return;

    try {
      await this.hubConnection.stop();
      console.log('SignalR disconnected');
    } catch (err) {
      console.error(err);
    }

    this.hubConnection = undefined;
    this.voiceAnalysisSubject.next(null);
  }

  // =====================================================
  // DESTROY
  // =====================================================

  ngOnDestroy(): void {

    this.stopConnection();
    this.authSubscription.unsubscribe();
    this.voiceAnalysisSubject.complete();

  }
}