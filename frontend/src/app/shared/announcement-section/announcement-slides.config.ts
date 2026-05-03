/** Parallel to `announcement.items` in i18n (same length). */
export const ANNOUNCEMENT_IMAGE_SRCS = [
  '/assets/announcement/announcement.jpg',
  '/assets/announcement/announcement-2.jpg',
  '/assets/announcement/announcement-3.jpg',
  '/assets/announcement/announcement-4.jpg',
] as const;

/** One Facebook URL per slide (can repeat). */
export const ANNOUNCEMENT_FACEBOOK_URLS = [
  "https://www.facebook.com/share/p/1CjiifLEfS/",
  "https://www.facebook.com/share/p/18XsbMAyeo/",
  "https://www.facebook.com/share/p/1AstEBYqax/",
  "https://www.facebook.com/share/p/1GSmyeXbsQ/"
] as const;

export type AnnouncementSlideI18n = {
  imageAlt: string;
  heading: string;
  subheading: string;
  paragraphs: string[];
  eventTitle: string;
  schedule: string;
  location: string;
};
