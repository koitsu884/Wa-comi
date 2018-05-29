import { NgModule } from "@angular/core";
import { Routes, RouterModule } from "@angular/router";
import { TopicListComponent } from "./topic-list/topic-list.component";
import { TopicListResolver } from "../_resolvers/topic-list.resolver";



const dailyTopicRoute: Routes = [
    {
        path: ':userId',
        runGuardsAndResolvers: 'always',
        component:TopicListComponent,
        // resolve: {
        //     topicList: TopicListResolver
        // },
    }
];

@NgModule({
    imports: [
        RouterModule.forChild(dailyTopicRoute)
    ],
    exports: [RouterModule]
})
export class DailyTopicRoutingModule {}