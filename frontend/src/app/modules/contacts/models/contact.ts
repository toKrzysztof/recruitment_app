import { ContactDto } from './contact-dto';

export type Contact = Omit<ContactDto, 'id'>;
