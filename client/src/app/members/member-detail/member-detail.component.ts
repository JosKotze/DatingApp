import { Component, inject, OnInit } from '@angular/core';
import { MembersService } from '../../service/members.service';
import { ActivatedRoute } from '@angular/router';
import { Member } from '../../models/member';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { GalleryItem, GalleryModule, ImageItem } from 'ng-gallery';

@Component({
  selector: 'app-member-detail',
  standalone: true,
  imports: [TabsModule, GalleryModule],
  templateUrl: './member-detail.component.html',
  styleUrl: './member-detail.component.css'
})
export class MemberDetailComponent implements OnInit {
  private memberService = inject(MembersService);
  private route = inject(ActivatedRoute);
  member?: Member;
  images: GalleryItem[] = [];

  ngOnInit(): void {
    this.loadMember();
  }

  loadMember(){
    const username = this.route.snapshot.paramMap.get('username');
    if(!username) return;
    this.memberService.getMember(username).subscribe({
      next: member => {
        this.member = member;
        // Ensure member.photo is defined and is an array
        if (member.photoUrl && Array.isArray(member.photos)) {
          this.images = member.photos.map(p => new ImageItem({ src: p.url, thumb: p.url }));
        } else {
          // Handle the case where photo is not defined or not an array
          this.images = [];
        }
      },
      error: err => {
        // Handle errors here if necessary
        console.error('Failed to load member', err);
      }
    });
  }

}
