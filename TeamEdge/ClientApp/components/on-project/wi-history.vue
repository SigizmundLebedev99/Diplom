<template>
    <div>
        <v-container v-show="loading">
            <v-layout column justify-center align-center fill-height>
                <v-progress-circular indeterminate color="primary"></v-progress-circular>
            </v-layout>
        </v-container>
        <div v-show="!loading">
            <v-layout justify-end>
                <v-btn icon @click="fetchHistory()">
                    <v-icon>
                        refresh
                    </v-icon>
                </v-btn>
            </v-layout>
            <v-container v-if="!history.length">
                <v-layout justify-center>
                    <span class>Изменений нет</span>
                </v-layout>
            </v-container>  
            <v-container v-else class="pt-0">
                
                <v-layout v-for="(h,i) in history" :key="i" class="mb-3">
                    <v-flex md6 sm8 xs10 offset-md3 offset-xs1 offset-sm2 class="history px-2 py-2">
                        <v-layout align-center wrap>
                            <user-label :user="h.initiator"></user-label><span class="mx-1">изменил</span>
                            <item-chip :item="h" :withoutRoute="true"/>
                            <v-spacer/>
                            <span class="date-text">{{getDate(h.dateOfCreation)}}</span>
                        </v-layout>
                        <v-divider class="mt-1"/>
                        <div v-for="(c,i) in h.changes" :key="i" class="my-3">
                            <v-layout column v-if="c.propertyName == 'Name'">
                                <span><strong>Название</strong> изменено с</span> 
                                <span class="label">"{{c.previous}}"</span>
                                <span> на </span>
                                <span class="label">"{{c.new}}"</span>
                            </v-layout>
                            <v-layout column v-if="c.propertyName == 'DescriptionText'">
                                <span><strong>Описание</strong> изменено с</span>
                                <strong v-if="!c.previous">-</strong>
                                <div v-else class="scroll-x overflow-x-hidden constraint" v-html="c.previous"></div>
                                <span>на</span>
                                <strong v-if="!c.new">-</strong>
                                <div v-else class="scroll-x overflow-x-hidden constraint" v-html="c.new"></div>
                            </v-layout>
                            <v-layout column v-if="c.propertyName == 'Files'">
                                <span><strong>Список вложенных файлов</strong> изменен</span>
                                <v-layout justify-space-around>
                                    <div>
                                        <span>Добавлено:</span>
                                        <v-layout align-center v-for="(f,i) in c.added" :key="i">
                                            <v-icon v-if="f.isPicture">
                                                image
                                            </v-icon>
                                            <v-icon v-else>
                                                insert_drive_file
                                            </v-icon>
                                            <span>{{f.Name}}</span>   
                                        </v-layout>
                                    </div>
                                    <div v-if="c.deleted && c.deleted.length">
                                        <span>Удалено:</span>
                                        <v-layout align-center v-for="(f,i) in c.deleted" :key="i">
                                            <v-icon v-if="f.isPicture">
                                                image
                                            </v-icon>
                                            <v-icon v-else>
                                                insert_drive_file
                                            </v-icon>
                                            <span>{{f.Name}}</span>   
                                        </v-layout>
                                    </div>
                                </v-layout>
                            </v-layout> 
                            <v-layout column v-if="c.propertyName == 'AssignedTo'">
                                <span><strong>Ислонитель</strong> изменен с</span>
                                
                                <v-layout align-center wrap>
                                    <strong v-if="!c.previous">-</strong>
                                    <user-label v-else :user="c.previous"></user-label>
                                    <span class="mx-2"> на </span>
                                    <strong v-if="!c.new">-</strong>
                                    <user-label v-else :user="c.new"></user-label>
                                </v-layout>
                            </v-layout>
                            <v-layout column v-if="c.propertyName == 'Parent'">
                                <span><strong>Предок</strong> изменен с</span>
                                <strong v-if="!c.previous">-</strong>
                                <item-chip v-else :item="c.previous"/>
                                <span class="mx-2"> на </span>
                                <strong v-if="!c.new">-</strong>
                                <item-chip v-else :item="c.new"/>
                            </v-layout>
                        </div>
                    </v-flex>
                </v-layout>
            </v-container>
        </div>
    </div>
</template>

<script>
import {mapGetters} from 'vuex'
import itemChip from './item-chip'
import userLabel from './user-label'
export default {
    components:{
        'user-label':userLabel,
        'item-chip':itemChip
    },
    mounted(){
        this.fetchHistory();
    },
    data:()=>({
        history:[],
        loading:false
    }),
    methods:{
        fetchHistory(){
            this.loading = true;
            var string = `/api/history/project/${this.$route.params.projId}`+
            `/code/${this.currentWI.code}/number/${this.currentWI.number}`
            this.$http.get(string).then(
                r=>{this.history = r.data; this.loading = false},
                r=>{console.log(r.response.data);this.loading = false;}
            )
        }
    },
    computed:{
        ...mapGetters({
            currentWI:'project/currentWI'
        })
    },
    watch:{
        currentWI(to, from){
            if(to)
                this.fetchHistory();
        }
    }
}
</script>
<style scoped>
.label{
    text-decoration: underline;
}
.constraint{
    max-height: 180px;
    background: white;
    padding: 4px;
}

.history{
    border-left: 3px solid grey;
    background-color: #dad6d6;
}
</style>
