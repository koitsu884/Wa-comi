import { Component, OnInit, ViewChild, ViewContainerRef } from '@angular/core';
import { Subscription } from 'rxjs/Subscription';
import { ModalService } from '../../_services/modal.service';

@Component({
  selector: 'app-modal',
  templateUrl: './modal.component.html',
  styleUrls: ['./modal.component.css']
})
export class ModalComponent implements OnInit {

  @ViewChild('inner', { read: ViewContainerRef }) vcr;
  private subscription: Subscription;
  public display = 'none';

  constructor(private modal: ModalService) { }

  ngAfterViewInit() {
    this.modal.vcr = this.vcr;
  }

  ngOnInit() {
    this.subscription = this.modal.content$.subscribe(
      value => {
        if (value) {
          this.display = '';
        } else {
          this.display = 'none';
        }
      });
  }

  containerClick($event) {
    $event.stopPropagation();
  }

  close() {
    this.modal.close();
  }

  ngOnDestroy() {
    this.subscription.unsubscribe();
  }

}
