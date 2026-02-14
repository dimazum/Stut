import { Component, OnInit } from '@angular/core';
import { VoiceAnalysisResult } from '../../models/models';
import { BackendService } from '../../services/backend.service';

@Component({
  selector: 'app-voise-anlysis-page',
  standalone: true,
  imports: [],
  templateUrl: './voise-anlysis-page.component.html',
  styleUrl: './voise-anlysis-page.component.css'
})
export class VoiseAnlysisPageComponent implements OnInit {
 voiceResults: VoiceAnalysisResult[] = [];

 constructor(private backendService: BackendService) {
 }
 
  ngOnInit() {
    this.backendService.getVoiceAnalysisResults()
      .subscribe(data =>{
        this.voiceResults = data;
        
      } );
  }
}
