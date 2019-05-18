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
                        <div v-for="(c,i) in h.changes" :key="i" class="my-2">
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
                                <v-layout v-if="c.added && c.added.length" align-center>
                                    <span>Добавлено:</span>
                                    <div class="flex-row mr-2" v-for="(f,i) in c.added" :key="i">
                                        <v-icon v-if="f.isPicture" class="center">
                                            image
                                        </v-icon>
                                        <v-icon v-else class="center">
                                            insert_drive_file
                                        </v-icon>
                                        <span class="center">{{f.Name}}</span>   
                                    </div>
                                </v-layout>
                                <v-layout v-if="c.deleted && c.deleted.length" align-center>
                                    <span>Удалено:</span>
                                    <div class="flex-row mr-2" v-for="(f,i) in c.deleted" :key="i">
                                        <v-icon v-if="f.isPicture" class="center">
                                            image
                                        </v-icon>
                                        <v-icon v-else class="center">
                                            insert_drive_file
                                        </v-icon>
                                        <span class="center">{{f.Name}}</span>   
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
                            <v-layout column v-if="c.propertyName == 'Children'">
                                <span><strong>Список дочерних единиц работы</strong> изменен</span>
                                <div v-if="c.added && c.added.length">
                                    <span>Добавлено:</span>
                                    <item-chip v-for="(w,i) in c.added" :key="i" :item="toLow(w)"/>
                                </div>
                                <div v-if="c.deleted && c.deleted.length">
                                    <span>Удалено:</span>
                                    <item-chip v-for="(w,i) in c.deleted" :key="i" :item="toLow(w)"/>
                                </div>
                            </v-layout>
                            <v-layout column v-if="c.propertyName == 'Epic'">
                                <span><strong>Epic link</strong> изменен с</span>
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
        },
        toLow(item){
            for(var key in item){
                var newKey = key[0].toLowerCase() + key.substring(1);
                item[newKey] = item[key];
            }
            return item;
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
