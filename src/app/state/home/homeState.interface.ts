import { IArticle } from '../../core/models/article.interface'
import { IFilter } from '../../core/models/filter.interface'
import { FeedType } from '../../core/ui-models/feed-types.enum'

export interface IHomeState {
  tags: string[]
  isLoadingTags: boolean
  isEditorFormSubmitting: boolean

  currentPage: number
  totalArticlesCount: number
  displayingArticles: IArticle[] | null
  isLoadingArticle: boolean
  feedType: FeedType
  filterBy: IFilter

  isSubmittingFavorite: boolean  
}
