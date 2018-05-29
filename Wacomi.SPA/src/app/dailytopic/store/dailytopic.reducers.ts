import * as DailyTopicActions from './dailytopic.actions';
import * as fromApp from '../../store/app.reducer';
import { DailyTopic } from '../../_models/DailyTopic';

export interface FeatureState extends fromApp.AppState {
    dailytopic : State
}

export interface State {
    topicList: DailyTopic[];
}

const initialState: State = {
    topicList: null
};

export function dailyTopicReducer(state = initialState, action: DailyTopicActions.DailyTopicActions ){
    let temp : DailyTopic[] = null;
    switch(action.type){
        case DailyTopicActions.SET_TOPIC_LIST:
            return {
                ...state,
                topicList: action.payload,
            };
        case DailyTopicActions.ADD_TOPIC:
            return {
                ...state,
                topicList: [...state.topicList, action.payload]
            }
        case DailyTopicActions.DELETE_TOPIC:
            temp = [...state.topicList];
            var index = temp.findIndex(x => x.id == action.payload);
            temp.splice(index, 1);
            return{
                ...state,
                topicList: temp
            };
        case DailyTopicActions.LIKE_TOPIC:
            temp = [...state.topicList];
            var index = temp.findIndex(x => x.id == action.payload.dailyTopicId);
            temp[index].isLiked = true;
            temp[index].likedCount++;
            return{
                ...state,
                topicList: temp
            };
        default:
            return state;
    }
}