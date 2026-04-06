export interface Festival {
  id: number;
  name: string;
  eventDate: string;
  venue: string;
  description: string;
  /** Optional English copy; client falls back to `description` when absent. */
  descriptionEn?: string;
  imageUrl?: string;
}
