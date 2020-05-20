import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-fetch-data',
  templateUrl: './home.component.html'
})
export class HomeComponent {
  public transactions: Transaction[];

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    http.get<Transaction[]>(baseUrl + 'api/transaction').subscribe(result => {
      this.transactions = result;
    }, error => console.error(error));
  }
}

interface Transaction {
  Id: string;
  Payment: string;
  Status: string;
}
