import { Injectable, OnDestroy } from '@angular/core';
import { BackendService } from './backend.service';

@Injectable({
  providedIn: 'root'
})
export class AudioRecorderService implements OnDestroy {
  private mediaRecorder!: MediaRecorder;
  private stream!: MediaStream;
  private recordedChunks: Blob[] = [];
  private isRecording = false;
  private uploadInterval!: any;

  constructor(private backendService: BackendService) {}

  startRecording(): void {
    if (this.isRecording) return;

    navigator.mediaDevices.getUserMedia({ audio: true })
      .then(stream => {
        this.stream = stream;
        this.isRecording = true;
        this.recordedChunks = [];

        this.startMediaRecorder();

        // Каждые 5 секунд отправляем данные
        this.uploadInterval = setInterval(() => {
          this.sendChunkToServer();
        }, 5000);
      })
      .catch(err => console.error('Microphone access error:', err));
  }

private startMediaRecorder(): void {
  this.mediaRecorder = new MediaRecorder(this.stream, { mimeType: 'audio/webm' });

  this.mediaRecorder.ondataavailable = event => {
    if (event.data.size > 0) {
      this.recordedChunks.push(event.data);
      console.log('chunk received', event.data.size);
    }
  };

  // 🔥 ВАЖНО
  this.mediaRecorder.start(3000); // каждые 5 сек браузер сам отдаёт Blob
}

  private sendChunkToServer(): void {
    if (this.recordedChunks.length === 0) return;

    this.mediaRecorder.stop();

    this.mediaRecorder.onstop = () => {
      const blob = new Blob(this.recordedChunks, { type: 'audio/webm' });
      this.recordedChunks = [];

      this.backendService.uploadRecording(blob).subscribe({
        next: res => console.log('Chunk uploaded', res),
        error: err => console.error('Upload error:', err)
      });

      this.startMediaRecorder();
    };
  }

  // -----------------------------
  // Новый метод: стоп без параметров
  // -----------------------------
  stop(): void {
    if (!this.isRecording) return;

    clearInterval(this.uploadInterval);

    this.mediaRecorder.onstop = () => {
      const blob = new Blob(this.recordedChunks, { type: 'audio/webm' });

      this.stream.getTracks().forEach(t => t.stop());
      this.isRecording = false;

      if (blob.size > 0) {
        this.backendService.uploadRecording(blob).subscribe({
          next: res => console.log('Final chunk uploaded', res),
          error: err => console.error('Upload error:', err)
        });
      }
    };

    this.mediaRecorder.stop();
  }

  ngOnDestroy(): void {
    if (this.isRecording) {
      this.stop();
    }
  }
}
