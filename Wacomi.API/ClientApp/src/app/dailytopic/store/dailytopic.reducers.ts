import * as DailyTopicActions from './dailytopic.actions';
import * as fromApp from '../../store/app.reducer';
import { DailyTopic } from '../../_models/DailyTopic';
import { TopicComment } from '../../_models/TopicComment';
import { TopicReply } from '../../_models/TopicReply';
import { TopicCommentFeel } from '../../_models/TopicCommentFeel';

export interface FeatureState extends fromApp.AppState {
    dailytopic: State
}

export interface State {
    topicList: DailyTopic[];
    likedTopicList: number[];
    topicComments: TopicComment[];
    commentFeelings: TopicCommentFeel[];
    topicReplies: { topicCommentId: number, replies: TopicReply[] }
    todaysComment: number;
}

const initialState: State = {
    topicList: null,
    likedTopicList: null,
    topicComments: [],
    commentFeelings: null,
    topicReplies: null,
    todaysComment: null
};

export function dailyTopicReducer(state = initialState, action: DailyTopicActions.DailyTopicActions) {
    let temp: DailyTopic[] = null;
    let tempTopicComments: TopicComment[] = null;
    switch (action.type) {
        case DailyTopicActions.TOPIC_CLEAR:
            return {
                ...state,
                topicList: null,
                topicComments: [],
                commentFeelings: null,
                topicReplies: null,
                todaysComment: null,
                likedTopicList: null,
            }
        //====================================================
        // Daily Topic Ranking
        //====================================================
        case DailyTopicActions.SET_TOPIC_LIST:
            return {
                ...state,
                topicList: action.payload,
            };
        case DailyTopicActions.SET_LIKED_TOPIC_LIST:
            temp = [...state.topicList];
            action.payload.forEach((topicId) => {
                var index = temp.findIndex(x => x.id == topicId);
                if (index >= 0) {
                    temp[index].isLiked = true;
                }
            });
            return {
                ...state,
                topicList: temp,
                likedTopicList: action.payload,
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
            return {
                ...state,
                topicList: temp
            };
        case DailyTopicActions.LIKE_TOPIC:
            temp = [...state.topicList];
            var index = temp.findIndex(x => x.id == action.payload.dailyTopicId);
            temp[index].isLiked = true;
            temp[index].likedCount++;
            return {
                ...state,
                topicList: temp,
                likedTopicList: [...state.likedTopicList, action.payload.dailyTopicId]
            };
        //====================================================
        // Topic Comment
        //====================================================
        case DailyTopicActions.GET_TOPIC_COMMENTS:
            return {
                ...state,
                topicComments: []
            }
        case DailyTopicActions.SET_TOPIC_COMMENTS:
            var myComment = action.payload.comments.find(c => c.appUserId == action.payload.appUserId);
            return {
                ...state,
                topicComments: action.payload.comments,
                todaysComment: myComment ? myComment.id : null
            };
        case DailyTopicActions.ADD_TOPIC_COMMENT:
            var newComment = action.payload;
            newComment.replyCount = 0;
            newComment.likedCount = 0;
            return {
                ...state,
                topicComments: [newComment, ...state.topicComments],
                todaysComment: newComment.id
            };
        case DailyTopicActions.DELETE_TOPIC_COMMENT:
            tempTopicComments = [...state.topicComments];
            var index = tempTopicComments.findIndex(x => x.id == action.payload);
            tempTopicComments.splice(index, 1);
            return {
                ...state,
                topicComments: tempTopicComments,
                todaysComment: state.todaysComment == action.payload ? null : state.todaysComment
            };
        case DailyTopicActions.SET_COMMENT_FEELINGS:
            tempTopicComments = [...state.topicComments];

            if (action.payload) {
                action.payload.forEach((feel) => {
                    var index = tempTopicComments.findIndex(x => x.id == feel.commentId);
                    if (index)
                        tempTopicComments[index].reactionByUser = feel.feeling;
                })
            }

            return {
                ...state,
                commentFeelings: action.payload,
                topicComments: tempTopicComments
            }
        case DailyTopicActions.ADD_COMMENT_FEELING:
            tempTopicComments = [...state.topicComments];
            var index = tempTopicComments.findIndex(x => x.id == action.payload.commentId);
            tempTopicComments[index].reactionByUser = action.payload.feeling;
            tempTopicComments[index].likedCount++;

            return {
                ...state,
                commentFeelings: [...state.commentFeelings, action.payload],
                topicComments: tempTopicComments
            }
        //====================================================
        // Topic Reply
        //====================================================
        case DailyTopicActions.TOGGLE_REPLY_FORM:
            tempTopicComments = [...state.topicComments];
            var index = tempTopicComments.findIndex(x => x.id == action.payload.commentId);
            tempTopicComments[index].displayReplies = !tempTopicComments[index].displayReplies;
            return {
                ...state,
                topicComments: tempTopicComments
            }
        case DailyTopicActions.SET_TOPIC_REPLIES:
            tempTopicComments = [...state.topicComments];
            var index = tempTopicComments.findIndex(x => x.id == action.payload.commentId);
            tempTopicComments[index].topicReplies = action.payload.topicReplies;
            tempTopicComments[index].replyCount = action.payload.topicReplies.length;

            return {
                ...state,
                topicComments: tempTopicComments
            }
        // case DailyTopicActions.ADD_TOPIC_REPLY:
        //     tempTopicComments = [...state.topicComments];
        //     var index = tempTopicComments.findIndex(x => x.id == action.payload.topicCommentId);
        //     tempTopicComments[index].topicReplies.push(action.payload);
        //     tempTopicComments[index].replyCount++;

        //     return {
        //         ...state,
        //         topicComments: tempTopicComments
        //     }
        case DailyTopicActions.DELETE_TOPIC_REPLY:
            tempTopicComments = [...state.topicComments];
            var index = tempTopicComments.findIndex(x => x.id == action.payload.topicCommentId);
            var tempTopicReplies = tempTopicComments[index].topicReplies;
            var replyIndex = tempTopicReplies.findIndex(x => x.id == action.payload.id);

            tempTopicReplies.splice(replyIndex, 1);
            tempTopicComments[index].topicReplies = tempTopicReplies;
            tempTopicComments[index].replyCount--;

            return {
                ...state,
                topicComments: tempTopicComments
            };
        //=======
        default:
            return state;
    }
}