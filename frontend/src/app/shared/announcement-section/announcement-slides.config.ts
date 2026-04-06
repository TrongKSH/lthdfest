/** Parallel to `announcement.items` in i18n (same length). */
export const ANNOUNCEMENT_IMAGE_SRCS = [
  '/assets/announcement/announcement.jpg',
  '/assets/announcement/announcement-2.jpg',
] as const;

/** One Facebook URL per slide (can repeat). */
export const ANNOUNCEMENT_FACEBOOK_URLS = [
  'https://www.facebook.com/share/p/14gg9mnZ1n9/',
  'https://www.facebook.com/share/p/14gg9mnZ1n9/',
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
