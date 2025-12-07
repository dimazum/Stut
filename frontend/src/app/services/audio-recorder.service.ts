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

  constructor(private backendService: BackendService) {}

  // -----------------------------
  // START RECORDING
  // -----------------------------
  startRecording(): void {
    if (this.isRecording) {
      return;
    }

    navigator.mediaDevices.getUserMedia({ audio: true })
      .then(stream => {
        this.stream = stream;
        this.recordedChunks = [];

        this.mediaRecorder = new MediaRecorder(stream, {
          mimeType: 'audio/webm'
        });

        this.mediaRecorder.ondataavailable = event => {
          if (event.data.size > 0) {
            this.recordedChunks.push(event.data);
          }
        };

        this.mediaRecorder.start();
        this.isRecording = true;
      })
      .catch(err => {
        console.error('Microphone access error:', err);
      });
  }

  // -----------------------------
  // STOP RECORDING
  // -----------------------------
  stopRecording(onStop: (blob: Blob) => void): void {
    if (!this.isRecording) {
      return;
    }

    this.mediaRecorder.onstop = () => {
      const blob = new Blob(this.recordedChunks, {
        type: 'audio/webm'
      });

      // остановить микрофон
      this.stream.getTracks().forEach(t => t.stop());

      this.isRecording = false;
      onStop(blob);
    };

    this.mediaRecorder.stop();
  }

  // -----------------------------
  // STOP + UPLOAD
  // -----------------------------
  stopAndUpload(onComplete?: (res: any) => void): void {
    this.stopRecording(blob => {
      this.backendService.uploadRecording(blob).subscribe({
        next: res => onComplete?.(res),
        error: err => console.error('Upload error:', err)
      });
    });
  }

  ngOnDestroy(): void {
    if (this.isRecording) {
      this.mediaRecorder.stop();
    }
  }
}
