import { Injectable } from '@angular/core';
import { LocalizationDto } from '../models/models';

@Injectable({ providedIn: 'root' })
export class LocalizationCacheService {
  private localizationCacheKey = 'app.localization-cache';

  public get(): LocalizationDto {
    const raw = localStorage.getItem(this.localizationCacheKey);

    return raw ? JSON.parse(raw) : null;
  }

  public set(dto: LocalizationDto): void {
    localStorage.setItem(this.localizationCacheKey, JSON.stringify(dto));
  }

  public clear(): void {
    localStorage.removeItem(this.localizationCacheKey);
  }
}
