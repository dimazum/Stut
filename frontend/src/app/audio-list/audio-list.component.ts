import { Component, OnInit } from '@angular/core';
import { BackendService } from '../services/backend.service';
import { AudioFile } from '../models/models';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-audio-list',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './audio-list.component.html',
  styleUrls: ['./audio-list.component.css']
})
export class AudioListComponent implements OnInit {
  files: AudioFile[] = [];

  currentFile?: AudioFile;
  audio = new Audio();
  progress = 0; // Прогресс в процентах

  constructor(private backendService: BackendService) {}

  ngOnInit(): void {
    this.loadFiles();

    // Обновляем прогресс при воспроизведении
    this.audio.ontimeupdate = () => {
      if (this.audio.duration) {
        this.progress = (this.audio.currentTime / this.audio.duration) * 100;
      }
    };

    // Сбрасываем прогресс при окончании трека
    this.audio.onended = () => {
      this.stop();
    };
  }

  loadFiles(): void {
    this.backendService.getList().subscribe(files => {
      this.files = files;
    });
  }

  play(file: AudioFile): void {
    const token = localStorage.getItem('token');
    if (!token) return;

    fetch(`/api/audio/file/${file.fileName}`, {
      headers: { 'Authorization': `Bearer ${token}` }
    })
      .then(res => {
        if (!res.ok) throw new Error('Ошибка загрузки аудио');
        return res.blob();
      })
      .then(blob => {
        const url = URL.createObjectURL(blob);

        // Останавливаем текущее аудио
        this.audio.pause();
        this.audio.currentTime = 0;

        this.audio.src = url;
        this.currentFile = file;
        this.audio.play();
      })
      .catch(err => console.error(err));
  }

  pause(): void {
    this.audio.pause();
  }

  stop(): void {
    this.audio.pause();
    this.audio.currentTime = 0;
    this.progress = 0;
    this.currentFile = undefined;
  }

  // Получение прогресса в %
  getProgress(): number {
    return this.progress || 0;
  }

  // Перемотка трека через input range
  seek(event: any): void {
    const value = event.target.value;
    if (this.audio.duration) {
      this.audio.currentTime = (value / 100) * this.audio.duration;
      this.progress = value;
    }
  }

  isPlaying(file: AudioFile): boolean {
    return this.currentFile?.fileName === file.fileName && !this.audio.paused;
  }

  delete(file: AudioFile): void {
  if (!confirm(`Удалить файл ${file.fileName}?`)) return;

  this.backendService.deleteFile(file.fileName).subscribe({
    next: () => {
      // Если удаляем текущий проигрываемый файл — остановим аудио
      if (this.currentFile?.fileName === file.fileName) {
        this.stop();
      }
      // Обновляем список файлов
      this.files = this.files.filter(f => f.fileName !== file.fileName);
    },
    error: (err) => console.error('Ошибка при удалении', err)
  });
}

}
