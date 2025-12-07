import { Component, ElementRef, OnDestroy, OnInit, ViewChild } from '@angular/core';
import AudioMotionAnalyzer from 'audiomotion-analyzer';
import { CommonModule } from '@angular/common';
import { startLessonSubject } from '../models/events';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-visualizer',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './visualizer-component.component.html',
  styles: [`
    .visualizer-container {
      width: 200px;
      height: 50px;
    }
    button {
      margin-top: 10px;
    }
  `]
})
export class VisualizerComponent implements  OnDestroy {
  @ViewChild('analyzerContainer', { static: true }) containerRef!: ElementRef<HTMLDivElement>;

  private audioMotion!: AudioMotionAnalyzer;
  private audioContext!: AudioContext;
  private source!: MediaStreamAudioSourceNode;
  private lessonSub!: Subscription;

  constructor() {
    this.lessonSub = startLessonSubject.subscribe(x => {
          if (x?.enabled) {
            this.start(false);
          } else {
            this.stop();
          }
        });
  }

async start(enableDelay: boolean = false) {

  const stream = await navigator.mediaDevices.getUserMedia({
    audio: { deviceId: { exact: 'default' } }
  });

  this.audioContext = new AudioContext();

  // Создаём узел микрофона
  this.source = this.audioContext.createMediaStreamSource(stream);

  // Настраиваем воспроизведение для наушников
  if (enableDelay) {
    const delayNode = this.audioContext.createDelay();
    delayNode.delayTime.value = 0.3; // задержка 2 секунды
    this.source.connect(delayNode);
    delayNode.connect(this.audioContext.destination);
  } else {
    // либо напрямую подключаемся, либо вообще не подключаем для тихого анализа
    //this.source.connect(this.audioContext.destination);
  }

  // Создаём AudioMotionAnalyzer для визуализации
  this.audioMotion = new AudioMotionAnalyzer(this.containerRef.nativeElement, {
    source: this.source,
    mode: 4,
    fftSize: 256,
    connectSpeakers: false
  });
}

  async stop() {
   this.audioMotion?.destroy();
    //this.audioContext?.close();
  }

  ngOnDestroy() {
    this.stop();
    this.lessonSub.unsubscribe();
  }
}
