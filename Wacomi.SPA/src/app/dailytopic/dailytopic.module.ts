import { NgModule } from "@angular/core";
import { TopicListComponent } from "./topic-list/topic-list.component";
import { TopicListResolver } from "../_resolvers/topic-list.resolver";
import { SharedModule } from "../shared/shared.module";
import { DailyTopicRoutingModule } from "./dailytopic-routing.module";
import { StoreModule } from "@ngrx/store";
import { dailyTopicReducer } from "./store/dailytopic.reducers";
import { EffectsModule } from "@ngrx/effects";
import { DailyTopicEffects } from "./store/dailytopic.effects";
import { TopicCommentListComponent } from "./topic-comment-list/topic-comment-list.component";
import { TopicCommentComponent } from "./topic-comment-list/topic-comment/topic-comment.component";


@NgModule({
    declarations: [
        TopicListComponent,
        TopicCommentListComponent,        
        TopicCommentComponent
    ],
    imports: [
        SharedModule,
        DailyTopicRoutingModule,
        StoreModule.forFeature('dailytopic', dailyTopicReducer),
        EffectsModule.forFeature([DailyTopicEffects])
    ],
    providers: [
        TopicListResolver
    ]
})

export class DailyTopicModule { }