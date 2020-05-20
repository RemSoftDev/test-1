import { Component, OnInit, Inject } from '@angular/core';
import { FileUploader, FileSelectDirective } from 'ng2-file-upload';

@Component({
  selector: 'app-fetch-data',
  templateUrl: './upload-data.component.html'
})
export class UploadDataComponent implements OnInit {
  title = 'Upload a File';

  public uploader: FileUploader;

  constructor(@Inject('BASE_URL') baseUrl: string) {
    this.uploader = new FileUploader({ url: baseUrl+'api/upload', itemAlias: 'transactions' })
  }

  ngOnInit() {
    this.uploader.onAfterAddingFile = (file) => { file.withCredentials = false; };
    this.uploader.onCompleteItem = (item: any, response: any, status: any, headers: any) => {
      console.log('FileUpload:uploaded:', item, status, response);
      alert('File uploaded successfully');
    };
  }
}

interface WeatherForecast {
  date: string;
  temperatureC: number;
  temperatureF: number;
  summary: string;
}
