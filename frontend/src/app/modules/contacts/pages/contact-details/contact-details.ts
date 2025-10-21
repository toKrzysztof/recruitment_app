import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ContactService } from '../../services/contact';
import { ContactDto } from '../../models/contact-dto';

@Component({
  selector: 'app-contact-details',
  imports: [],
  templateUrl: './contact-details.html',
  styleUrl: './contact-details.scss'
})
export class ContactDetailsComponent implements OnInit {
  protected contact: ContactDto | null = null;

  constructor(private contactService: ContactService, private route: ActivatedRoute) {}

  ngOnInit(): void {
    const id = +this.route.snapshot.paramMap.get('id')!;
    this.contactService.getContact(id).subscribe((contact) => {
      this.contact = contact;
    });
  }
}
