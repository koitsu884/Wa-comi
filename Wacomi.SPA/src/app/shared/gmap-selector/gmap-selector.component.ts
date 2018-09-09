import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { Marker, Circle, LatLngLiteral } from '@agm/core/services/google-maps-types';
import { MouseEvent } from '@agm/core';
import { AlertifyService } from '../../_services/alertify.service';

@Component({
  selector: 'app-gmap-selector',
  templateUrl: './gmap-selector.component.html',
  styleUrls: ['./gmap-selector.component.css']
})
export class GmapSelectorComponent implements OnInit {
  @Input() lat: number;
  @Input() lng: number;
  @Input() radius: number;
  @Input() useCircle: boolean;
  marker: Marker;
  circle: Circle;

  @Output() areaSelectedEvent = new EventEmitter<{ lat: number, lng: number, radius: number }>();

  constructor(private alertify: AlertifyService) { }

  ngOnInit() {
      //Default position: Britomart
    if (!this.lat)
      this.lat = -36.8441;
    if (!this.lng)
      this.lng = 174.7678;
    if (!this.radius)
      this.radius = 1000;
  }

  mapClicked($event: MouseEvent) {
    this.lng = $event.coords.lng;
    this.lat = $event.coords.lat;
    this.areaChanged();
  }

  radiusChange($event: number) {
    if($event > 5000)
    {
      this.alertify.error("選択範囲が広すぎます");
      this.radius = 5000;
      return;
    }
    this.areaChanged();
  }

  markerDragEnd($event: MouseEvent) {
    this.lng = $event.coords.lng;
    this.lat = $event.coords.lat;
    this.areaChanged();
  }

  centerChanged($event: LatLngLiteral){
    this.lng = $event.lng;
    this.lat = $event.lat;
    this.areaChanged();
  }

  private areaChanged() {
    this.areaSelectedEvent.emit({ lat: this.lat, lng: this.lng, radius: this.useCircle ? this.radius : null });
  }

}
