import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-weatherservice',
  templateUrl: './weatherservice.component.html',
  styleUrls: ['./weatherservice.component.css']
})
export class WeatherserviceComponent implements OnInit {

  values: any;

  constructor(private http: HttpClient ) { }

  ngOnInit() {
    this.getValues();
  }

  getValues()
  {
    this.http.get('http://localhost:64654/weatherforecast').subscribe(response => {this.values = response; },
       error => { console.log(error); } );
  }

}
