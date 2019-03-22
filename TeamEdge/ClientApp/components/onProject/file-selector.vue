<template>
    <v-bottom-sheet v-model="opened" persistent>
        <v-toolbar dense class="primary" dark>
            <v-toolbar-title class="title">Добавление файлов</v-toolbar-title>
            <v-spacer></v-spacer>
            <v-btn icon @click="setOpened(false)">
                <v-icon>close</v-icon>
            </v-btn>
        </v-toolbar>
        <div class="white">
            <v-layout column class="mr-3 ml-3">
                <v-layout row align-center wrap>
                    <span class="subheading mt-1">Прикрепите имеющиеся в проекте файлы или загрузите новые</span>
                    <v-spacer></v-spacer>
                    <v-icon>search</v-icon>
                    <div class="constraint">
                        <v-text-field></v-text-field>
                    </div>
                </v-layout>
                <v-layout row wrap>
                    <v-flex md-4 xs12>

                    </v-flex>
                    <v-flex md-4 xs12>
                        
                    </v-flex>
                    <v-flex md-4 xs12>
                        <v-container v-show="loading">
                            <v-layout column justify-center align-center fill-height>
                                <v-progress-circular indeterminate color="primary"></v-progress-circular>
                            </v-layout>
                        </v-container>
                        <v-list>
                            
                        </v-list>
                    </v-flex>
                </v-layout>
            </v-layout>
        </div>
    </v-bottom-sheet>
</template>

<script>
import {mapGetters, mapMutations} from 'vuex'
export default {
    data:()=>({
        
    }),
    methods:{
        ...mapMutations({
            setOpened:'fileSelector/setOpened'
        })
    },
    computed:{
        ...mapGetters({
            opened:'fileSelector/opened',
            project:'project/project',
            loading:'fileSelector/loading'
        })
    }
}
</script>

<style scoped>
.constraint{
    width: 200px;
}

.white{
    background: white;
}
</style>

