import { NgModule } from "@angular/core";
import { Routes, RouterModule } from "@angular/router";
import { TopicListComponent } from "./topic-list/topic-list.component";
import { TopicListResolver } from "../_resolvers/topic-list.resolver";
import { TopicCommentListComponent } from "./topic-comment-list/topic-comment-list.component";
import { AppUserResolver } from "../_resolvers/appuser.resolver";



const dailyTopicRoute: Routes = [
    {
        path: 'ranking/:userId',
        runGuardsAndResolvers: 'always',
        component:TopicListComponent,
        // resolve: {
        //     topicList: TopicListResolver
        // },
    },
    {
        path: ':topicCommentId',
        runGuardsAndResolvers: 'always',
        component:TopicCommentListComponent,
        resolve: {
            appUser: AppUserResolver
        }
    },
    {
        path: '',
        runGuardsAndResolvers: 'always',
        component:TopicCommentListComponent,
        resolve: {
            appUser: AppUserResolver
        }
    }
];

@NgModule({
    imports: [
        RouterModule.forChild(dailyTopicRoute)
    ],
    exports: [RouterModule]
})
export class DailyTopicRoutingModule {}